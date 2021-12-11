using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class DbInitializer
    {
        public static void Seed(RepositoryContext context)
        {
            context.Database.EnsureCreated();


            if (!context.Roles.Any(x => x.Name == "SuperAdmin"))
            {
                var superAdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                };

                context.Roles.Add(superAdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = superAdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }
            
            if (!context.Roles.Any(x => x.Name == "Administrateur"))
            {
                var AdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrateur",
                    NormalizedName = "ADMINISTRATEUR"
                };

                context.Roles.Add(AdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = AdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }


            if (!context.Roles.Any(x => x.Name == "Client"))
            {
                var AdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Client",
                    NormalizedName = "CLIENT"
                };

                context.Roles.Add(AdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = AdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }


            if (!context.Roles.Any(x => x.Name == "Organisateur"))
            {
                var AdminRole = new Workstation
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Organisateur",
                    NormalizedName = "Organisateur"
                };

                context.Roles.Add(AdminRole);

                ClaimsStore.AllClaims.ForEach(claim =>
                {
                    var claimWrapper = new ClaimWrapper();
                    claimWrapper.RoleId = AdminRole.Id;
                    claimWrapper.InitializeFromClaim(claim);

                    context.RoleClaims.Add(claimWrapper);
                });
            }


            context.SaveChanges();
        }
    }
}
