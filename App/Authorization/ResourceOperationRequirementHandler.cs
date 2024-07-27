using App.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace App.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Review>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Review review)        
        {

            if(requirement.OperationType == OperationType.Read || requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }


            if (requirement.OperationType == OperationType.Delete || requirement.OperationType == OperationType.Update)
            {
                var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

                if(userId == review.ReviewedBy.Id)
                {
                    context.Succeed(requirement);
                }

            }

            return Task.CompletedTask;


        }
    }
}
