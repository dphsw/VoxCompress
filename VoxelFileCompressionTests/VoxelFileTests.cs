using System;
using System.IO;
using System.Diagnostics;
using Xunit;
using VoxelFileCompression;

namespace VoxelFileCompressionTests {
  public class VoxelFileTests {

    const string TestFilename = "..\\..\\..\\menger243.vox";

    [Fact]
    public void TestFileExists() {
      Assert.NotEqual("", TestFilename);
      Assert.NotEqual("", File.ReadAllText(TestFilename));
    }

    [Fact]
    public void TestFileIsMagicaVoxelFile() {
      VoxelFile voxelFile = GetTestVoxelFile();
      Assert.False(voxelFile.IsCompressed);
      Assert.True(voxelFile.VersionIndicator > 0);
      Assert.True(voxelFile.FileSize >= 20);
      Assert.True(voxelFile.GetNumChunks() > 1);
    }

    [Fact]
    public void CanSaveAndLoadTestFile() {
      VoxelFile voxelFile = GetTestVoxelFile();

      VoxelFile newVoxelFile = SaveThenLoad(voxelFile);

      MemoryStream oldFile = new MemoryStream();
      MemoryStream newFile = new MemoryStream();
      voxelFile.Save(oldFile);
      newVoxelFile.Save(newFile);
      Assert.True(BitConverter.ToString(oldFile.ToArray()) == BitConverter.ToString(newFile.ToArray()));
    }

    [Fact]
    public void CanCompressFile() {
      VoxelFile voxelFile = GetTestVoxelFile();
      VoxelFile compressed = voxelFile.MakeCopy(true);

      Assert.False(voxelFile.IsCompressed);
      Assert.True(compressed.IsCompressed);
      Assert.True(compressed.GetNumChunks() == voxelFile.GetNumChunks());

      // This is not strictly always true - it depends on using a
      // test file with a reasonably sized voxel model in it.
      Assert.True(compressed.FileSize < voxelFile.FileSize);
    }

    [Fact]
    public void CanDecompressFile() {
      VoxelFile voxelFile = GetTestVoxelFile();
      VoxelFile compressed = voxelFile.MakeCopy(true);
      VoxelFile decompressed = compressed.MakeCopy(false);

      Assert.True(decompressed.FileSize == voxelFile.FileSize);
      Assert.True(decompressed.IsCompressed == voxelFile.IsCompressed);
      Assert.True(decompressed.GetNumChunks() == voxelFile.GetNumChunks());
    }

    private VoxelFile GetTestVoxelFile() {
      VoxelFile voxelFile;
      using (FileStream fs = new FileStream(TestFilename, FileMode.Open)) voxelFile = VoxelFile.Load(fs);
      return voxelFile;
    }

    private VoxelFile SaveThenLoad(VoxelFile voxelFile) {
      int fileSize = voxelFile.FileSize;
      byte[] buffer = new byte[fileSize];
      using (MemoryStream ms = new MemoryStream(buffer)) voxelFile.Save(ms);

      VoxelFile newFile;
      using (MemoryStream ms = new MemoryStream(buffer)) newFile = VoxelFile.Load(ms);
      return newFile;
    }

  }
}
