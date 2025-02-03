﻿using Backend.Models;
using Backend.Models.Enums;


namespace Backend
{
    public interface IClothingItemsService
    {
        (ExecutionStatus status, ClothingItem? newClothingItem) AddClothingItem(ClothingItem clothingItem);
        (ExecutionStatus status, List<ClothingItem> clothingItems) FindAllClothingItems();
        (ExecutionStatus status, ClothingItem ClothingItem) FindClothingItemById(int id);
        ExecutionStatus DeleteClothingItem(int id);
        (ExecutionStatus status, ClothingItem clothingItem) ReplaceClothingItem(int id, ClothingItem clothingItem);
        (ExecutionStatus status, List<ClothingItem> clothingItems) GetFilteredClothingItems(ClothingItemFilter filter);
    }
}
