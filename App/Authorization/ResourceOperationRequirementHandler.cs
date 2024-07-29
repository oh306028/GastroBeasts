using App.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace App.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, object>   
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, object resource)          
        {

            if(requirement.OperationType == OperationType.Read || requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }


            if (requirement.OperationType == OperationType.Delete || requirement.OperationType == OperationType.Update)
            {
                var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);


                if(resource is Review review)
                {
                    if (userId == review.ReviewedBy.Id)
                    {
                        context.Succeed(requirement);
                    }

                }

                if (resource is Restaurant restaurant)
                {
                    if (userId == restaurant.UserId)
                    {
                        context.Succeed(requirement);
                    }

                }

            }

            return Task.CompletedTask;


        }
    }
}
