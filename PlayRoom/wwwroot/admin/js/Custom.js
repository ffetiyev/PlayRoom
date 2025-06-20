"use strict"

let discoutDeleteBtns = document.querySelectorAll(".discount-delete-btn");

discoutDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let discountId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Discount/Delete?id=" + discountId,{

        method:"POST",

        })
        .then(response => response.text()).then(res => {
            this.parentNode.parentNode.remove()
            })
        })

}))


let specialBannerDeleteBtns = document.querySelectorAll(".banner-delete-btn");

specialBannerDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let bannerId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/SpecialGameBanner/Delete?id=" + bannerId, {

            method: "POST",

        })
            .then(async (response) => {
                if (!response.ok) {
                    let errorText = await response.text();
                    alert(errorText);
                    return;
                }
                this.parentNode.parentNode.remove()
            })
    })

}))

let setActiveBannerImageBtn = document.querySelector(".set-active-btn");

if (setActiveBannerImageBtn) {
    setActiveBannerImageBtn.addEventListener("click", function () {
        let button = this;
        let container = button.parentNode;
        let bannerId = parseInt(container.getAttribute("data-id"));

        fetch("http://localhost:5125/Admin/SpecialGameBanner/SetActiveBanner?id=" + bannerId, {
            method: "POST"
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to activate banner.");
                }
                return response.text();
            })
            .then(() => {
                // Replace the button with the green "Active" badge
                container.innerHTML = `
                <div style="width: 90px; background-color: limegreen; color: white; border-radius: 7px; padding: 5px 0px 5px 20px;" class="mt-3 d-block">
                    Active
                </div>`;
            })
            .catch(err => {
                console.error(err);
                alert("Failed to set active. Please try again.");
            });
    });
}


let companyDeleteBtns = document.querySelectorAll(".company-delete-btn");

companyDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let companyId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Company/Delete?id=" + companyId, {

            method: "POST",

        })
            .then(async (response) => {
                if (!response.ok) {
                    let errorText = await response.text();
                    alert(errorText);
                    return;
                }
                this.parentNode.parentNode.remove()
            })
    })

}))

let categoryDeleteBtns = document.querySelectorAll(".category-delete-btn");

categoryDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let categoryId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Category/Delete?id=" + categoryId, {

            method: "POST",
        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })
}))


let gameDeleteBtns = document.querySelectorAll(".game-delete-btn");

gameDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let gameId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Game/Delete?id=" + gameId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))



let gameImageDeleteBtns = document.querySelectorAll(".game-image-delete-btn");

gameImageDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let imageId = parseInt(this.parentNode.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Game/DeleteGameImage?id=" + imageId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))


function bindSetMainButtons() {
    let gameImageSetMainBtns = document.querySelectorAll(".game-image-set-main-btn");

    gameImageSetMainBtns.forEach(btn => {
        btn.addEventListener("click", function () {
            let imageId = parseInt(this.closest(".buttons").getAttribute("data-id"));

            fetch("/Admin/Game/SetMainImage?id=" + imageId, {
                method: "POST"
            })
                .then(response => {
                    if (!response.ok) throw new Error("Failed to set main image.");
                    return response.text();
                })
                .then(() => {
                    document.querySelectorAll(".image-edit-game").forEach(el => {
                        el.classList.remove("image-edit-game");
                        el.querySelector(".main-image-show")?.remove();

                        const buttons = el.querySelector(".buttons");
                        buttons.innerHTML = `
                            <button class="btn btn-danger game-image-delete-btn">Delete</button>
                            <button class="btn btn-success game-image-set-main-btn">Set main</button>
                        `;
                    });

                    const imageContainer = this.closest(".image-edit");
                    imageContainer.classList.add("image-edit-game");

                    const buttons = this.closest(".buttons");
                    buttons.innerHTML = `<span class="badge badge-success main-image-show">Main Image</span>`;

                    bindSetMainButtons();
                })
                .catch(err => {
                    console.error("Set main image error:", err);
                });
        });
    });
}

bindSetMainButtons();


let consoleDeleteBtns = document.querySelectorAll(".console-delete-btn");

consoleDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let consoleId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Console/Delete?id=" + consoleId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))

function consoleSetMainButtons() {
    let consoleImageSetMainBtns = document.querySelectorAll(".console-image-set-main-btn");

    // Remove previous event listeners by cloning each button
    consoleImageSetMainBtns.forEach(btn => {
        const newBtn = btn.cloneNode(true);
        btn.parentNode.replaceChild(newBtn, btn);
    });

    // Re-select buttons after replacement
    consoleImageSetMainBtns = document.querySelectorAll(".console-image-set-main-btn");

    consoleImageSetMainBtns.forEach(btn => {
        btn.addEventListener("click", function () {
            let imageId = parseInt(this.closest(".buttons").getAttribute("data-id"));

            fetch("/Admin/console/SetMainImage?id=" + imageId, {
                method: "POST"
            })
                .then(response => {
                    if (!response.ok) throw new Error("Failed to set main image.");
                    return response.text();
                })
                .then(() => {
                    // Reset all images
                    document.querySelectorAll(".image-edit-game").forEach(el => {
                        el.classList.remove("image-edit-game");
                        el.querySelector(".main-image-show")?.remove();

                        const buttons = el.querySelector(".buttons");
                        buttons.innerHTML = `
                            <button class="btn btn-danger game-image-delete-btn">Delete</button>
                            <button class="btn btn-success console-image-set-main-btn">Set main</button>
                        `;
                    });

                    // Highlight the newly selected main image
                    const imageContainer = this.closest(".image-edit");
                    imageContainer.classList.add("image-edit-game");

                    const buttons = this.closest(".buttons");
                    buttons.innerHTML = `<span class="badge badge-success main-image-show">Main Image</span>`;

                    // Rebind for new buttons
                    consoleSetMainButtons();
                })
                .catch(err => {
                    console.error("Set main image error:", err);
                });
        });
    });
}

// Initial binding on page load
consoleSetMainButtons();


let consoleImageDeleteBtns = document.querySelectorAll(".console-image-delete-btn");

consoleImageDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let imageId = parseInt(this.parentNode.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/console/DeleteConsoleImage?id=" + imageId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))

let accessoryDeleteBtns = document.querySelectorAll(".accessory-delete-btn");

accessoryDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let accessoryId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Accessory/Delete?id=" + accessoryId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))



let accessoryImageDeleteBtns = document.querySelectorAll(".accessory-image-delete-btn");

accessoryImageDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let imageId = parseInt(this.parentNode.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Accessory/DeleteGameImage?id=" + imageId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))


document.addEventListener("click", function (e) {
    if (e.target.classList.contains("accessory-image-set-main-btn")) {
        const btn = e.target;
        const imageId = parseInt(btn.closest(".buttons").getAttribute("data-id"));

        fetch("/Admin/Accessory/SetMainImage?id=" + imageId, {
            method: "POST"
        })
            .then(response => {
                if (!response.ok) throw new Error("Failed to set main image.");
                return response.text();
            })
            .then(() => {
                document.querySelectorAll(".image-edit-game").forEach(el => {
                    el.classList.remove("image-edit-game");
                    el.querySelector(".main-image-show")?.remove();

                    const buttons = el.querySelector(".buttons");
                    buttons.innerHTML = `
                        <button class="btn btn-danger accessory-image-delete-btn">Delete</button>
                        <button class="btn btn-success accessory-image-set-main-btn">Set main</button>
                    `;
                });

                const imageContainer = btn.closest(".image-edit");
                imageContainer.classList.add("image-edit-game");

                const buttons = btn.closest(".buttons");
                buttons.innerHTML = `<span class="badge badge-success main-image-show">Main Image</span>`;
            })
            .catch(err => {
                console.error("Set main image error:", err);
            });
    }
});



let newsImageDeleteBtns = document.querySelectorAll(".news-delete-btn");

newsImageDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let newsId = parseInt(this.parentNode.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/News/Delete?id=" + newsId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))


let promocodeDeleteBtns = document.querySelectorAll(".promocode-delete-btn");

promocodeDeleteBtns.forEach((btn => {
    btn.addEventListener("click", function () {
        let newsId = parseInt(this.getAttribute("data-id"));
        fetch("http://localhost:5125/Admin/Promocode/Delete?id=" + newsId, {

            method: "POST",

        })
            .then(response => response.text()).then(res => {
                this.parentNode.parentNode.remove()
            })
    })

}))