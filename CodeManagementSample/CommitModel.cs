using System;

namespace CodeManagementSample;

public record CommitModel(string Id, string Author, string Message, DateTime Time);
