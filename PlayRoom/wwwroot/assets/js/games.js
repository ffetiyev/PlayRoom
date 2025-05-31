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


let typeFilterButton = document.querySelector("#main-top .product-filters .filters .type-filter");

typeFilterButton.addEventListener("click",function(){
   let types = document.querySelector("#main-top .product-filters .filters .type-filter .types");
   if(types.classList.contains("d-none")){
    types.classList.remove("d-none")
   }
   else{
    types.classList.add("d-none")
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

let allButtons = document.querySelectorAll("#main-top .pagination-part .pagination a");
let numberButtons = Array.from(allButtons).slice(1, -1); // Exclude first and last

numberButtons.forEach(element => {
  element.addEventListener("click", function(event) {

    let activeNumber = document.querySelector("#main-top .pagination-part .pagination .active");
    if (activeNumber) {
      activeNumber.classList.remove("active");
    }

    element.classList.add("active");
  });
});


//document.querySelectorAll(".pagination a.page-value").forEach(btn => {
//    btn.addEventListener("click", function (e) {
//        e.preventDefault();
//        let page = this.dataset.id;

//        fetch(`/Games/GetPaginatedGames?page=${page}`)
//            .then(res => res.json())
//            .then(data => {
//                let container = document.querySelector(".products .row");
//                container.innerHTML = "";

//                data.datas.forEach(item => {
//                    let discount = item.gameDiscounts.length > 0;
//                    let discountAmount = discount ? Math.max(...item.gameDiscounts.map(d => d.value)) : 0;
//                    let finalPrice = discount ? item.price - (item.price * discountAmount / 100) : item.price;

//                    container.innerHTML += `
//                        <div class="col-lg-3 col-6 product">
//                            <div class="card-main">
//                                <div class="card-top">
//                                    <a href="#">
//                                        <img src="/assets/images/Game-Images/${item.gameImages.find(img => img.isMain).name}" alt="">
//                                    </a>
//                                    ${discount ? '<div class="discount"><span>ENDIRIM</span></div>' : ''}
//                                    ${new Date(item.createdDate) > new Date(new Date().setFullYear(new Date().getFullYear() - 1)) ? '<div class="new"><span>YENI</span></div>' : ''}
//                                    <div class="like"><i class="fa-solid fa-heart"></i></div>
//                                </div>
//                                <div class="card-bottom">
//                                    <div class="game-name">
//                                        <a href="#">${item.name}</a>
//                                    </div>
//                                    <div class="game-price mt-4">
//                                        ${discount ? `<span class="old-price">${item.price.toFixed(0)} ?</span>` : ''}
//                                        <span class="price">${finalPrice.toFixed(0)} ?</span>
//                                    </div>
//                                </div>
//                                <div class="add-basket">
//                                    <button>S?B?T? AT</button>
//                                </div>
//                            </div>
//                        </div>
//                    `;
//                });
//            });
//    });
//});





