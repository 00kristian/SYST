using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst {
public static class Extensions
{
    public static ActionResult ToActionResult(this Status status) => status switch
    {
        Status.Created => new OkResult(),
        Status.Updated => new NoContentResult(),
        Status.Deleted => new NoContentResult(),
        Status.NotFound => new NotFoundResult(),
        Status.Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{status} not supported")
    };
} }