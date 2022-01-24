using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CPW211_EntityFrameworkQueries
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectAllVendors_Click(object sender, EventArgs e)
        {
            using APContext dbContext = new APContext();

            // LINQ (Language Integrated Query) method syntax
            List<Vendor> vendorList = dbContext.Vendors.ToList();

            // LINQ query syntax
            List<Vendor> vendorList2 = (from v in dbContext.Vendors
                                        select v).ToList();
        }

        private void btnAllCaliVendors_Click(object sender, EventArgs e)
        {
            using APContext dbContext = new();

            List<Vendor> vendorList = dbContext.Vendors
                                        .Where(v => v.VendorState == "CA")
                                        .OrderBy(v => v.VendorName)
                                        .ToList();

            List<Vendor> vendorList2 = (from v in dbContext.Vendors
                                        where v.VendorState == "CA"
                                        orderby v.VendorName
                                        select v).ToList();
                
        }

        private void btnSelectSpecificColumns_Click(object sender, EventArgs e)
        {
            APContext dbContext = new();
            // Anonymous type
            List<VendorLocation> results = (from v in dbContext.Vendors
                           select new VendorLocation
                           {
                               VendorName = v.VendorName,
                               VendorState = v.VendorState,
                               VendorCity = v.VendorCity
                           }).ToList();

            StringBuilder displayString = new();
            foreach(VendorLocation vendor in results)
            {
                displayString.AppendLine($"{vendor.VendorName} is in {vendor.VendorCity}");
            }

            MessageBox.Show(displayString.ToString());
        }

        private void btnMiscQueries_Click(object sender, EventArgs e)
        {
            APContext dbContext = new();

            // Check if a Vendor exists in Washington
            bool doesExist = (from v in dbContext.Vendors
                              where v.VendorState == "WA"
                              select v).Any();

            // Get number of Invoices
            int invoiceCount = (from invoice in dbContext.Invoices
                                select invoice).Count();

            // Query a single Vendor
            Vendor ? singleVendor = (from v in dbContext.Vendors
                                     where v.VendorName == "Joe's Burger Shack"
                                     select v).SingleOrDefault();

            if (singleVendor != null)
            {
                // Do something with the Vendor object
            }
        }

        private void btnVendorsAndInvoices_Click(object sender, EventArgs e)
        {
            APContext dbContext = new();

            // Vendors LEFT JOIN Invoices
            List<Vendor> allVendors = dbContext.Vendors.Include(v => v.Invoices).ToList();

            // Unfinished code: This pulls a Vendor object for each individual invoice, vendors
            // are also pulled back if they have no invoices
            //List<Vendor> allVendors = (from v in dbContext.Vendors
            //                          join inv in dbContext.Invoices
            //                            on v.VendorId equals inv.VendorId into grouping
            //                          from inv in grouping.DefaultIfEmpty()
            //                          select v).ToList();

            StringBuilder results = new();

            foreach(Vendor v in allVendors)
            {
                results.Append(v.VendorName);
                foreach(Invoice inv in v.Invoices)
                {
                    results.Append(", ");
                    results.Append(inv.InvoiceNumber);
                }
                results.AppendLine();
            }

            MessageBox.Show(results.ToString());
        }
    }

    class VendorLocation
    {
        public string VendorName { get; set; }

        public string VendorState { get; set; }

        public string VendorCity { get; set; }
    }
}