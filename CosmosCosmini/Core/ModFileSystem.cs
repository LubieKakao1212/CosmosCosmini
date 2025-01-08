using JustLoaded.Filesystem;

namespace CosmosCosmini.Core;

public class ModFileSystem(IFilesystem fs) {

    public readonly IFilesystem filesystem = fs;

}