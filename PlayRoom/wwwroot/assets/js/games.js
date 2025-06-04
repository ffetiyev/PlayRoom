"use strict"

let categoryFilterButton = document.querySelector("#main-top .product-filters .filters .category");

categoryFilterButton.addEventListener("click",function(){
   let categories = document.querySelector("#main-top .product-filters .filters .filter-main .categories");
   if(categories.classList.contains("d-none")){
    categories.classList.remove("d-none")
   }
   else{
    categories.classList.add("d-none")
   }
})

let priceFilterButton = document.querySelector("#main-top .product-filters .filters .price-filter");

priceFilterButton.addEventListener("click",function(){
   let prices = document.querySelector("#main-top .product-filters .filters .price-filter .prices");
   if(prices.classList.contains("d-none")){
    prices.classList.remove("d-none")
   }
   else{
    prices.classList.add("d-none")
   }
})

let orderFilterButton = document.querySelector("#main-top .product-filters .filters .order-filter");

orderFilterButton.addEventListener("click",function(){
   let orders = document.querySelector("#main-top .product-filters .filters .order-filter .orders");
   if(orders.classList.contains("d-none")){
    orders.classList.remove("d-none")
   }
   else{
    orders.classList.add("d-none")
   }
})

