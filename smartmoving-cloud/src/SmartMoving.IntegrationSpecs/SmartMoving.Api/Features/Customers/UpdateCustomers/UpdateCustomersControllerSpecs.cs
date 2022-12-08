using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Should;
using SmartMoving.Api.Features.Customers.Common;
using SmartMoving.Api.Features.Customers.UpdateCustomer;
using SmartMoving.Api.Features.Paging;
using SmartMoving.Core.Data.Core;
using SmartMoving.Data.Contexts;
using SmartMoving.IntegrationSpecs.Builders;
using SmartMoving.IntegrationSpecs.Helpers;
using SmartMoving.IntegrationSpecs.Infrastructure;
using SpecsFor.Core.ShouldExtensions;

namespace SmartMoving.IntegrationSpecs.SmartMoving.Api.Features.Customers.GetCustomers
{
    public class UpdateCustomersControllerSpecs
    {
        public class WhenUpdatingCustomerDetails : SpecsForAsync<UpdateCustomerController>, ICurrentUserCustomer
        {
            private Customer _customer;
            private ActionResult<CustomerDetailsViewModel> _response;
            private CustomerDetailsViewModel _result;

            public AppDbContext Context { get; set; }

            protected override async Task GivenAsync()
            {
                _customer = await Context.BuildCustomer(x =>
                                         {
                                             x.Address = "123 Customer Address";
                                             x.EmailAddress = "test@dummy.com";
                                             x.HasAccount = false;
                                             x.Name = "MC Movesalot";
                                             x.EmployeeNumber = "SM-1234567-234567891";
                                             x.PhoneNumber = "1234567890";
                                             x.PhoneType = PhoneType.Mobile;
                                             x.SecondaryPhoneNumbers = new[]
                                             {
                                                 new SecondaryPhoneNumber
                                                 {
                                                     PhoneNumber = "9876543210",
                                                     PhoneType = PhoneType.Home
                                                 }
                                             };
                                         })
                                         .FinishAsync();

                // other customer
                await Context.BuildCustomer().FinishAsync();

                await Context.SaveChangesAsync();
            }

            protected override async Task WhenAsync()
            {
                _response = await SUT.UpdateCustomer(_customer.Id, new EditCustomerForm()
                {
                    Address = "123 Customer Address",
                    EmailAddress = "test@dummy.com",
                    Name = "Dummy Customer",
                    EmployeeNumber = "SM-1234567-234567891",
                    PhoneNumber = "1234567890",
                    PhoneType = PhoneType.Mobile,
                    SecondaryPhoneNumbers = new[]
                    {
                            new SecondaryPhoneNumber
                            {
                                PhoneNumber = "9876543210",
                                PhoneType = PhoneType.Home
                            }
                    }
                });
                    
                _result = _response.TryGetObjectFromResult();
            }

            [Test]
            public void ThenItReturnsOk()
            {
                _response.Result.ShouldBeType<OkObjectResult>();
            }

            [Test]
            public void ThenItPopulatesOtherAttributesCorrectly()
            {
                _result.ShouldLookLike(() => new CustomerDetailsViewModel
                {
                    Address = "123 Customer Address",
                    EmailAddress = "test@dummy.com",
                    Name = "Dummy Customer",
                    EmployeeNumber = "SM-1234567-234567891",
                    PhoneNumber = "1234567890",
                    PhoneType = PhoneType.Mobile,
                    SecondaryPhoneNumbers = new[]
                    {
                            new SecondaryPhoneNumber
                            {
                                PhoneNumber = "9876543210",
                                PhoneType = PhoneType.Home
                            }
                    }
                });
            }
        }
    }
}