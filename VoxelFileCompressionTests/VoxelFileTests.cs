using System;
using System.IO;
using System.Diagnostics;
using Xunit;
using VoxelFileCompression;

namespace VoxelFileCompressionTests {
  public class VoxelFileTests {

    const string TestFilename = "..\\..\\..\\realistic_terrain.vox";

    [Fact]
    public void TestFileExists() {
      Assert.NotEqual("", TestFilename);
      Assert.NotEqual("", File.ReadAllText(TestFilename));
    }

    [Fact]
    public void TestFileIsMagicaVoxelFile() {
      using (FileStream fs = new FileStream(TestFilename, FileMode.Open)) {
        VoxelFile voxelFile = VoxelFile.Load(fs);
        Assert.False(voxelFile.CompressedWhenLoaded);
        Assert.True(voxelFile.VersionIndicator > 0);
        Assert.True(voxelFile.FileSize >= 20);
        Assert.True(voxelFile.GetNumChunks() > 1);
      }
    }

    [Fact]
    public void CanSaveAndLoadTestFile() {
      VoxelFile voxelFile;
      using (FileStream fs = new FileStream(TestFilename, FileMode.Open)) {
        voxelFile = VoxelFile.Load(fs);
      }

      VoxelFile newVoxelFile = SaveThenLoad(voxelFile);

      Assert.True(newVoxelFile.CompressedWhenLoaded == voxelFile.CompressedWhenLoaded);
      Assert.True(newVoxelFile.FileSize == voxelFile.FileSize);
      Assert.True(newVoxelFile.VersionIndicator == newVoxelFile.VersionIndicator);
      Assert.True(newVoxelFile.GetNumChunks() == newVoxelFile.GetNumChunks());

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
