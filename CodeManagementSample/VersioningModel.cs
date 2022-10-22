using System;

namespace CodeManagementSample;

public record VersioningModel(string Author, Guid BlobId, string BlobContent, string RepositoryPath);
