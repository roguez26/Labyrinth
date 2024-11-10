using CatalogManagementService;
using LabyrinthCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class CatalogManagementServiceTest
    {
        private CatalogManagementServiceImplementation _catalogManagementServiceImplementation;
        private readonly List<TransferCountry> _testCountries = new List<TransferCountry>
        {
            new TransferCountry { CountryId = 1, CountryName = "Mexico" },
            new TransferCountry { CountryId = 2, CountryName = "USA" },
            new TransferCountry { CountryId = 3, CountryName = "Canada" }
        };

        [TestInitialize]
        public void SetUp()
        {
            _catalogManagementServiceImplementation = new CatalogManagementServiceImplementation();

            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.Country.RemoveRange(context.Country.ToList());
                context.SaveChanges();

                foreach (var testCountry in _testCountries)
                {
                    var newCountry = new DataAccess.Country
                    {
                        name = testCountry.CountryName
                    };
                    context.Country.Add(newCountry);
                    context.SaveChanges();

                    testCountry.CountryId = newCountry.idCountry;
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new DataAccess.LabyrinthEntities())
            {
                context.Country.RemoveRange(context.Country.ToList());
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void TestGetAllCountriesSuccesful()
        {
            List<TransferCountry> obtainedCountries = _catalogManagementServiceImplementation.GetAllCountries();

            Assert.IsNotNull(obtainedCountries);
            Assert.AreEqual(_testCountries.Count, obtainedCountries.Count);

            foreach (var expectedCountry in _testCountries)
            {
                var matchingCountry = obtainedCountries.FirstOrDefault(country => country.CountryId == expectedCountry.CountryId);

                Assert.IsNotNull(matchingCountry);
                Assert.AreEqual(expectedCountry.CountryName, matchingCountry.CountryName);
            }
        }

        [TestMethod]
        public void TestGetAllCountriesFailure()
        { 
            _testCountries.Add(new TransferCountry { CountryId = 1, CountryName = "Argentina"});
            List<TransferCountry> obtainedCountries = _catalogManagementServiceImplementation.GetAllCountries();
            Assert.IsNotNull(obtainedCountries);
            Assert.AreNotEqual(_testCountries.Count, obtainedCountries.Count);
        }

        [TestMethod]
        public void TestGetCountryByIDSuccesful()
        {
            var expectedCountry = _testCountries.First();
            TransferCountry obtainedCountry = _catalogManagementServiceImplementation.GetCountryById(expectedCountry.CountryId);

            Assert.AreEqual(expectedCountry.CountryId, obtainedCountry.CountryId);
            Assert.AreEqual(expectedCountry.CountryName, obtainedCountry.CountryName);
        }

        [TestMethod]
        public void TestGetCountryByIDFailureByIDNotFound()
        {
            TransferCountry obtainedCountry = _catalogManagementServiceImplementation.GetCountryById(9999);
            Assert.IsNotNull(obtainedCountry);
            Assert.AreEqual(0, obtainedCountry.CountryId);
        }

    }
}
