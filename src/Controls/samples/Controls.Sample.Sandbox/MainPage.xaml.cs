using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	const int ItemCount = 30;
	readonly List<MultiBinding> _rootedBindings = [];

	public MainPage()
	{
		InitializeComponent();
	}

	void OnRunReproductionClicked(object sender, EventArgs e)
	{
		_rootedBindings.Clear();

		var controlReferences = CreateScenario(Scenario.Control);
		var leakyReferences = CreateScenario(Scenario.Leaky);
		var mitigationReferences = CreateScenario(Scenario.Mitigation);

		ForceGc();

		var controlAlive = controlReferences.Count(reference => reference.IsAlive);
		var leakyAlive = leakyReferences.Count(reference => reference.IsAlive);
		var mitigationAlive = mitigationReferences.Count(reference => reference.IsAlive);

		ControlResultLabel.Text = $"Control: {controlAlive} / {ItemCount} payloads alive";
		LeakyResultLabel.Text = $"Applied binding: {leakyAlive} / {ItemCount} payloads alive";
		MitigationResultLabel.Text = $"RemoveBinding: {mitigationAlive} / {ItemCount} payloads alive";
		ReproductionSummaryLabel.Text = leakyAlive == ItemCount && controlAlive == 0 && mitigationAlive == 0
			? "Issue reproduced: rooted MultiBinding retains its target."
			: "Issue not reproduced with the observed alive counts.";

		Console.WriteLine($"SANDBOX: Issue 38017 counts: {controlAlive} / {leakyAlive} / {mitigationAlive}");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	List<WeakReference> CreateScenario(Scenario scenario)
	{
		var references = new List<WeakReference>(ItemCount);

		for (var index = 0; index < ItemCount; index++)
			references.Add(CreateItem(scenario));

		return references;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	WeakReference CreateItem(Scenario scenario)
	{
		var label = new PayloadLabel();
		var binding = new MultiBinding { StringFormat = "{0}" };
		binding.Bindings.Add(new Binding(".", source: "value"));
		_rootedBindings.Add(binding);

		if (scenario != Scenario.Control)
			label.SetBinding(Label.TextProperty, binding);
		if (scenario == Scenario.Mitigation)
			label.RemoveBinding(Label.TextProperty);

		return new WeakReference(label.Payload);
	}

	static void ForceGc()
	{
		for (var iteration = 0; iteration < 7; iteration++)
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}

	sealed class PayloadLabel : Label
	{
		public byte[] Payload { get; } = new byte[1024 * 1024];
	}

	enum Scenario
	{
		Control,
		Leaky,
		Mitigation
	}
}