namespace ExpenseCareApi.Core.DTOs;

public class ApproveDto
{
    public string ApprovedBy { get; set; } = string.Empty;
}

public class RejectDto
{
    public string RejectedBy { get; set; } = string.Empty;
}

public class ApproveAllDto
{
    public List<int> Ids { get; set; } = new();
    public string ApprovedBy { get; set; } = string.Empty;
}