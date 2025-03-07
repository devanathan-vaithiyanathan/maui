using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Xunit;

namespace Microsoft.Maui.Essentials.DeviceTests
{
	[Category("FileSystem")]
	public class FileSystem_Tests
	{
		const string BundleFileContents = "This file was in the app bundle.";

		[Fact]
		public void CacheDirectory_Is_Valid()
		{
			Assert.False(string.IsNullOrWhiteSpace(FileSystem.CacheDirectory));
		}

		[Fact]
		public void AppDataDirectory_Is_Valid()
		{
			Assert.False(string.IsNullOrWhiteSpace(FileSystem.AppDataDirectory));
		}

		[Theory]
		[InlineData("AppBundleFile.txt", BundleFileContents)]
		[InlineData("AppBundleFile_NoExtension", BundleFileContents)]
		[InlineData("Folder/AppBundleFile_Nested.txt", BundleFileContents)]
		[InlineData("Folder\\AppBundleFile_Nested.txt", BundleFileContents)]
		public async Task OpenAppPackageFileAsync_Can_Load_File(string filename, string contents)
		{
			using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
			Assert.NotNull(stream);

			using var reader = new StreamReader(stream);
			var text = await reader.ReadToEndAsync().ConfigureAwait(false);

			Assert.Equal(contents, text);
		}

		[Fact]
		public async Task OpenAppPackageFileAsync_Throws_If_File_Is_Not_Found()
		{
			await Assert.ThrowsAsync<FileNotFoundException>(() => FileSystem.OpenAppPackageFileAsync("MissingFile.txt")).ConfigureAwait(false);
		}

		[Fact]
		public async Task CheckFileResultWithFilePath()
		{
			string filePath = Path.Combine(FileSystem.CacheDirectory, "sample.txt");
			await File.WriteAllTextAsync(filePath, "Sample content for testing");

			var fileResult = new FileResult(filePath);

			using var stream = await fileResult.OpenReadAsync();

			Assert.NotNull(stream);

			var bytes = new byte[stream.Length];
			_ = await stream.ReadAsync(bytes, 0, (int)stream.Length);

			Assert.True(bytes.Length > 0);
			File.Delete(filePath);
		}
	}
}
