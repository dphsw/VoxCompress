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

  }
}
