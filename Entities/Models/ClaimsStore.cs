using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>
        {
            new Claim("readBanner", "Read Banners"),
            new Claim("writeBanner", "Write Banners"),
            new Claim("manageBanner", "Manage Banners"),


            new Claim("readCategory", "Read Categories"),
            new Claim("writeCategory", "Write Categories"),
            new Claim("manageCategory", "Manage Categories"),

            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),
            new Claim("manageAppUser", "Manage AppUsers"),

            new Claim("readCategoryBlog", "Read CategoryBlog"),
            new Claim("writeCategoryBlog", "Write CategoryBlog"),
            new Claim("manageCategoryBlog", "Manage CategoryBlog"),

            new Claim("readBlog", "Read Blog"),
            new Claim("writeBlog", "Write Blog"),
            new Claim("manageBlog", "Manage Blog"),

            new Claim("readOrder", "Read Order"),
            new Claim("writeOrder", "Write Order"),
            new Claim("manageOrder", "Manage Order"),

            new Claim("readComment", "Read Comment"),
            new Claim("writeComment", "Write Comment"),
            new Claim("manageComment", "Manage Comment"),

            new Claim("readSubCategory", "Read SubCategory"),
            new Claim("writeSubCategory", "Write SubCategory"),
            new Claim("manageSubCategory", "Manage SubCategory"),

            new Claim("readCommercial", "Read Commercial"),
            new Claim("writeCommercial", "Write Commercial"),
            new Claim("manageCommercial", "Manage Commercial"),

            new Claim("readEvent", "Read Event"),
            new Claim("writeEvent", "Write Event"),
            new Claim("manageEvent", "Manage Event"),

            new Claim("readPaymentType", "Read PaymentType"),
            new Claim("writePaymentType", "Write PaymentType"),
            new Claim("managePaymentType", "Manage PaymentType"),

            new Claim("readPayment", "Read Payment"),
            new Claim("writePayment", "Write Payment"),
            new Claim("managePayment", "Manage Payment"),

            new Claim("readPlace", "Read Place"),
            new Claim("writePlace", "Write Place"),
            new Claim("managePlace", "Manage Place"),

            new Claim("readPartner", "Read Partner"),
            new Claim("writePartner", "Write Partner"),
            new Claim("managePartner", "Manage Partner"),

            new Claim("readPromote", "Read Promote"),
            new Claim("writePromote", "Write Promote"),
            new Claim("managePromote", "Manage Promote"),

            new Claim("readPromoteEvent", "Read PromoteEvent"),
            new Claim("writePromoteEvent", "Write PromoteEvent"),
            new Claim("managePromoteEvent", "Manage PromoteEvent"),

            new Claim("readEventYear", "Read EventYear"),
            new Claim("writeEventYear", "Write EventYear"),
            new Claim("manageEventYear", "Manage EventYear"),

            new Claim("readSponsor", "Read Sponsor"),
            new Claim("writeSponsor", "Write Sponsor"),
            new Claim("manageSponsor", "Manage Sponsor"),

            new Claim("readWorkstation", "Read Workstations"),
            new Claim("writeWorkstation", "Write Workstations"),
            new Claim("manageWorkstation", "Manage Workstations"),
        };

        public static List<Claim> OrganisatorClaims = new List<Claim>
        {
            new Claim("readBanner", "Read Banners"),

            new Claim("readCategory", "Read Categories"),

            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),

            new Claim("readCategoryBlog", "Read CategoryBlog"),

            new Claim("readBlog", "Read Blog"),

            new Claim("readOrder", "Read Order"),
            new Claim("writeOrder", "Write Order"),

            new Claim("readComment", "Read Comment"),

            new Claim("readSubCategory", "Read SubCategory"),

            new Claim("readCommercial", "Read Commercial"),

            new Claim("readEvent", "Read Event"),
            new Claim("writeEvent", "Write Event"),

            new Claim("readPaymentType", "Read PaymentType"),

            new Claim("readPayment", "Read Payment"),
            new Claim("writePayment", "Write Payment"),

            new Claim("readPlace", "Read Place"),
            new Claim("writePlace", "Write Place"),

            new Claim("readPartner", "Read Partner"),

            new Claim("readPromote", "Read Promote"),

            new Claim("readPromoteEvent", "Read PromoteEvent"),
            new Claim("writePromoteEvent", "Write PromoteEvent"),

            new Claim("readEventYear", "Read EventYear"),

            new Claim("readSponsor", "Read Sponsor"),
            new Claim("writeSponsor", "Write Sponsor"),

            new Claim("readWorkstation", "Read Workstations"),
        };

        public static List<Claim> AdministratorClaims = new List<Claim>
        {

             new Claim("readBanner", "Read Banners"),
            new Claim("writeBanner", "Write Banners"),
            new Claim("manageBanner", "Manage Banners"),


            new Claim("readCategory", "Read Categories"),
            new Claim("writeCategory", "Write Categories"),
            new Claim("manageCategory", "Manage Categories"),

            new Claim("readCategoryBlog", "Read CategoryBlog"),
            new Claim("writeCategoryBlog", "Write CategoryBlog"),
            new Claim("manageCategoryBlog", "Manage CategoryBlog"),

            new Claim("readBlog", "Read Blog"),
            new Claim("writeBlog", "Write Blog"),
            new Claim("manageBlog", "Manage Blog"),

            new Claim("readOrder", "Read Order"),
            new Claim("writeOrder", "Write Order"),
            new Claim("manageOrder", "Manage Order"),

            new Claim("readComment", "Read Comment"),
            new Claim("writeComment", "Write Comment"),
            new Claim("manageComment", "Manage Comment"),

            new Claim("readSubCategory", "Read SubCategory"),
            new Claim("writeSubCategory", "Write SubCategory"),
            new Claim("manageSubCategory", "Manage SubCategory"),

            new Claim("readCommercial", "Read Commercial"),
            new Claim("writeCommercial", "Write Commercial"),
            new Claim("manageCommercial", "Manage Commercial"),

            new Claim("readEvent", "Read Event"),
            new Claim("writeEvent", "Write Event"),
            new Claim("manageEvent", "Manage Event"),

            new Claim("readPaymentType", "Read PaymentType"),
            new Claim("writePaymentType", "Write PaymentType"),
            new Claim("managePaymentType", "Manage PaymentType"),

            new Claim("readPayment", "Read Payment"),
            new Claim("writePayment", "Write Payment"),
            new Claim("managePayment", "Manage Payment"),

            new Claim("readPlace", "Read Place"),
            new Claim("writePlace", "Write Place"),
            new Claim("managePlace", "Manage Place"),

            new Claim("readPartner", "Read Partner"),
            new Claim("writePartner", "Write Partner"),
            new Claim("managePartner", "Manage Partner"),

            new Claim("readPromote", "Read Promote"),
            new Claim("writePromote", "Write Promote"),
            new Claim("managePromote", "Manage Promote"),

            new Claim("readPromoteEvent", "Read PromoteEvent"),
            new Claim("writePromoteEvent", "Write PromoteEvent"),
            new Claim("managePromoteEvent", "Manage PromoteEvent"),

            new Claim("readEventYear", "Read EventYear"),
            new Claim("writeEventYear", "Write EventYear"),
            new Claim("manageEventYear", "Manage EventYear"),

            new Claim("readSponsor", "Read Sponsor"),
            new Claim("writeSponsor", "Write Sponsor"),
            new Claim("manageSponsor", "Manage Sponsor"),

            new Claim("readWorkstation", "Read Workstations"),
            new Claim("writeWorkstation", "Write Workstations"),
            new Claim("manageWorkstation", "Manage Workstations"),
        };

        public static List<Claim> ClientClaims = new List<Claim>
        {
            new Claim("readBanner", "Read Banners"),

            new Claim("readCategory", "Read Categories"),

            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),

            new Claim("readCategoryBlog", "Read CategoryBlog"),

            new Claim("readBlog", "Read Blog"),

            new Claim("readOrder", "Read Order"),
            new Claim("writeOrder", "Write Order"),

            new Claim("readComment", "Read Comment"),
            new Claim("writeComment", "Write Comment"),

            new Claim("readSubCategory", "Read SubCategory"),

            new Claim("readCommercial", "Read Commercial"),

            new Claim("readEvent", "Read Event"),

            new Claim("readPaymentType", "Read PaymentType"),

            new Claim("readPayment", "Read Payment"),
            new Claim("writePayment", "Write Payment"),

            new Claim("readPlace", "Read Place"),

            new Claim("readPartner", "Read Partner"),

            new Claim("readPromote", "Read Promote"),

            new Claim("readPromoteEvent", "Read PromoteEvent"),

            new Claim("readEventYear", "Read EventYear"),

            new Claim("readSponsor", "Read Sponsor"),

            new Claim("readWorkstation", "Read Workstations"),
        };
    }
}
