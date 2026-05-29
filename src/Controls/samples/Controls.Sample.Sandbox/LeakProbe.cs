using System.Collections.Concurrent;

namespace Maui.Controls.Sample;

static class LeakProbe
{
	sealed class ProbeEntry
	{
		public required string Id { get; init; }
		public required string Kind { get; init; }
		public required DateTime CreatedUtc { get; init; }
		public required WeakReference Target { get; init; }
		public string LastState { get; set; } = "created";
		public DateTime LastStateUtc { get; set; } = DateTime.UtcNow;
	}

	static readonly ConcurrentDictionary<string, ProbeEntry> s_entries = new();
	static int s_id;

	public static string Track(object target, string kind)
	{
		var id = $"{kind}-{Interlocked.Increment(ref s_id)}";
		s_entries[id] = new ProbeEntry
		{
			Id = id,
			Kind = kind,
			CreatedUtc = DateTime.UtcNow,
			Target = new WeakReference(target)
		};

		Log($"track {id}");
		return id;
	}

	public static void Mark(string? id, string state)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}

		if (s_entries.TryGetValue(id, out var entry))
		{
			entry.LastState = state;
			entry.LastStateUtc = DateTime.UtcNow;
		}

		Log($"{id} => {state}");
	}

	public static string BuildSummary()
	{
		var entries = s_entries.Values.ToArray();
		if (entries.Length == 0)
		{
			return "No tracked objects yet.";
		}

		var now = DateTime.UtcNow;
		var alive = entries.Where(e => e.Target.IsAlive).ToArray();

		var perKind = alive
			.GroupBy(e => e.Kind)
			.OrderBy(g => g.Key)
			.Select(g => $"{g.Key}:{g.Count()}");

		var oldestAliveAge = alive.Length == 0
			? TimeSpan.Zero
			: now - alive.Min(e => e.CreatedUtc);

		return $"Tracked={entries.Length} Alive={alive.Length} Dead={entries.Length - alive.Length} AliveByKind=[{string.Join(", ", perKind)}] OldestAlive={oldestAliveAge.TotalSeconds:F1}s";
	}

	public static string BuildAliveDetails(int maxItems = 24)
	{
		var alive = s_entries.Values
			.Where(e => e.Target.IsAlive)
			.OrderBy(e => e.CreatedUtc)
			.Take(maxItems)
			.Select(e =>
			{
				var age = DateTime.UtcNow - e.CreatedUtc;
				return $"{e.Id} state={e.LastState} age={age.TotalSeconds:F1}s";
			});

		var list = alive.ToArray();
		if (list.Length == 0)
		{
			return "No alive tracked objects.";
		}

		return string.Join(Environment.NewLine, list);
	}

	public static void Log(string message)
	{
		var line = $"[LEAK-PROBE] {DateTime.Now:HH:mm:ss.fff} {message}";
		Console.WriteLine(line);
		System.Diagnostics.Debug.WriteLine(line);
	}
}