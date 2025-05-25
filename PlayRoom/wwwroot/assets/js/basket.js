"use strict"
var QtyInput = (function () {
	var $qtyInputs = $(".qty-input");

	if (!$qtyInputs.length) {
		return;
	}

	var $inputs = $qtyInputs.find(".product-qty");
	var $countBtn = $qtyInputs.find(".qty-count");
	var qtyMin = parseInt($inputs.attr("min"));
	var qtyMax = parseInt($inputs.attr("max"));

	$inputs.change(function () {
		var $this = $(this);
		var $minusBtn = $this.siblings(".qty-count--minus");
		var $addBtn = $this.siblings(".qty-count--add");
		var qty = parseInt($this.val());

		if (isNaN(qty) || qty <= qtyMin) {
			$this.val(qtyMin);
			$minusBtn.attr("disabled", true);
		} else {
			$minusBtn.attr("disabled", false);
			
			if(qty >= qtyMax){
				$this.val(qtyMax);
				$addBtn.attr('disabled', true);
			} else {
				$this.val(qty);
				$addBtn.attr('disabled', false);
			}
		}
	});

	$countBtn.click(function () {
		var operator = this.dataset.action;
		var $this = $(this);
		var $input = $this.siblings(".product-qty");
		var qty = parseInt($input.val());

		if (operator == "add") {
			qty += 1;
			if (qty >= qtyMin + 1) {
				$this.siblings(".qty-count--minus").attr("disabled", false);
			}

			if (qty >= qtyMax) {
				$this.attr("disabled", true);
			}
		} else {
			qty = qty <= qtyMin ? qtyMin : (qty -= 1);
			
			if (qty == qtyMin) {
				$this.attr("disabled", true);
			}

			if (qty < qtyMax) {
				$this.siblings(".qty-count--add").attr("disabled", false);
			}
		}

		$input.val(qty);
	});
})();


// Toggle promo code section
let addPromoCodeBtn = document.querySelector("#main-top .basket-main .promocode .main-part .add-promo-item .add");
let removePromoCodeBtn = addPromoCodeBtn.nextElementSibling;
let promocodeSection = document.querySelector("#main-top .basket-main .promocode .add-promocode");

addPromoCodeBtn.addEventListener("click", function () {
    promocodeSection.classList.remove("d-none");
    addPromoCodeBtn.classList.add("d-none");
    removePromoCodeBtn.classList.remove("d-none");
});

removePromoCodeBtn.addEventListener("click", function () {
    promocodeSection.classList.add("d-none");
    addPromoCodeBtn.classList.remove("d-none");
    removePromoCodeBtn.classList.add("d-none");
});

// Handle promo code submission
let applyButton = document.querySelector(".promocode .confirm-code button");
let promoInput = document.querySelector(".promocode .promo-input");

applyButton.addEventListener("click", async function () {
    const code = promoInput.value.trim();

    if (!code) {
        alert("Zəhmət olmasa promokod daxil edin.");
        return;
    }

    // Example API call — replace '/api/apply-promo' with your actual endpoint
    const response = await fetch("/api/apply-promo", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ promoCode: code })
    });

    const result = await response.json();

    if (result.success) {
        alert(`Promokod tətbiq olundu: ${result.discount}% endirim`);
        // You can update price/total here
    } else {
        alert("Promokod etibarsızdır.");
    }
});

