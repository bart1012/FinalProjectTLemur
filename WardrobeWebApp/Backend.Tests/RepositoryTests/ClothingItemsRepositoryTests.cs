﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Backend;
using Backend.Models;
using Backend.Models.Enums;
using Backend.Tests;
using System.Dynamic;

namespace Backend.Tests.RepositoryTests
{
    internal class ClothingItemsRepositoryTests
    {
        WardrobeDBContext _dbContext;
        ClothingItemsRepository _repository;
        ClothingItem _initialClothingItem;
        ClothingItem _testClothingItem;



        [TearDown]
        public void TeardownDb()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        public void InjectInitialTestDataIntoDb(WardrobeDBContext dbContext)
        {

            _initialClothingItem = TestExamples.GetInitialClothingItem();


            if (dbContext != null)
            {
                dbContext.ClothingItems.Add(_initialClothingItem);
                dbContext.SaveChanges();
                return;
            }
            throw new NullReferenceException("No database context present");
        }

        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WardrobeDBContext>();
            optionsBuilder.UseInMemoryDatabase("WardrobeInMemory");
            _dbContext = new WardrobeDBContext(optionsBuilder.Options);
            InjectInitialTestDataIntoDb(_dbContext);
            _repository = new ClothingItemsRepository(_dbContext);

            _testClothingItem = TestExamples.GetADifferentClothingItem();
        }


        [Test]
        public void AddNewClothingItem_Increases_Count_Of_ClothingItems_By_One()
        {
            // Arrange
            var clothingItemsCount = _repository.FindAllClothingItems().Count();

            // Act
            var addedClothingItem = _repository.AddClothingItem(_testClothingItem);

            // Assert
            Assert.That(_repository.FindAllClothingItems().Count() == clothingItemsCount + 1);
        }


        [Test]
        public void DeleteClothingItem_Returns_Null_If_Album_Does_Not_Exist()
        {

            // Arrange
            var initialClothingItem = _repository.FindAllClothingItems().FirstOrDefault();
            var id = initialClothingItem?.Id;

            // Act
             _repository.DeleteClothingItem(_initialClothingItem);

            // Asserta
            if (id is null)
            {
                Assert.That(false);
            }
            else 
            {
                Assert.That(_repository.FindClothingItemById((int)id), Is.Null);
            };
           
        }

        [Test]
        public void FindAllClothingItems_Returns_List_of_Albums_Not_Null()
        {
            // Arrange


            // Act
            var clothingItems = _repository.FindAllClothingItems();

            // Assert
            Assert.That(clothingItems, Is.Not.Null);
            Assert.That(clothingItems.GetType() == typeof(List<ClothingItem>));
        }

        [Test]
        public void FindClothingItemById_Returns_Not_Null()
        {
            // Arrange


            // Act
            var album = _repository.FindClothingItemById(1);

            // Assert
            Assert.That(album != null);
        }

        [Test]
        public void ReplaceClothingItem_Returns_ClothingItemWithUpdatedProperties()
        {
            // Arrange
            var clothingItem = _repository.FindAllClothingItems().First();
            var originalSize = clothingItem.Size;
            var originalId = clothingItem.Id;

            clothingItem.Size = ClothingSize.L;

            // Act
            var updatedClothingItem = _repository.ReplaceClothingItem(clothingItem);

            // Assert
            Assert.That(updatedClothingItem, Is.Not.Null);
            Assert.That(updatedClothingItem.Size == ClothingSize.L &&
                            updatedClothingItem.Size != originalSize &&
                            updatedClothingItem.Id == originalId);
        }
    }
}
