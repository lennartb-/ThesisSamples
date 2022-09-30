using System;

namespace CodeManagementSample;

public record VersioningModel(string Author, Guid BlobId, object BlobContent, string RepositoryPath);
