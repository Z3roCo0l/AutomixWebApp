// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


$(document).ready(function () {
    // Smooth scrolling
    $("a[href^='#']").on("click", function (event) {
        event.preventDefault();
        $("html, body").animate({
            scrollTop: $($.attr(this, "href")).offset().top
        }, 500);
    });

    // Navbar background change on scroll
    $(window).scroll(function () {
        if ($(window).scrollTop() > 50) {
            $(".navbar").addClass("scrolled");
            $(".logo").css("font-size", "1em");
        } else {
            $(".navbar").removeClass("scrolled");
            $(".logo").css("font-size", "1.5em");
        }
    });

// Back to top button
$("body").append("<div class='back-to-top'></div>");
$(".back-to-top").css({
    "display": "none",
    "position": "fixed",
    "bottom": "20px",
    "right": "20px",
    "width": "40px",
    "height": "40px",
    "background-color": "#333",
    "color": "#fff",
    "border-radius": "50%",
    "text-align": "center",
    "line-height": "40px",
    "font-size": "24px",
    "cursor": "pointer",
}).text("^");

$(window).scroll(function () {
    if ($(window).scrollTop() > 300) {
        $(".back-to-top").fadeIn();
    } else {
        $(".back-to-top").fadeOut();
    }
});

$(".back-to-top").on("click", function () {
    $("html, body").animate({ scrollTop: 0 }, 500);
});
});


// Language switcher
const languageButtons = document.querySelectorAll('.language-switcher button');
const menuItems = document.querySelectorAll('nav a');
const contentSections = document.querySelectorAll('section');

languageButtons.forEach(button => {
    button.addEventListener('click', () => {
        const lang = button.getAttribute('data-lang');

        // Update menu items
        menuItems.forEach((item, index) => {
            item.textContent = contentSections[index].getAttribute(`data-${lang}`);
        });

        // Update content sections
        contentSections.forEach(section => {
            section.textContent = section.getAttribute(`data-${lang}`);
        });
    });
});

//DailyMenuCarousel
//function initializeweeklymenucarousel() {
//    console.log('initializing weekly menu carousel')
//    const weekstoshow = 3; // number of weeks to display in the carousel
//    const today = new date();
//    const startofweek = new date(today.setdate(today.getdate() - today.getday() + (today.getday() === 0 ? -6 : 1)));

//    for (let i = 0; i < weekstoshow; i++) {
//        const weekstart = new date(startofweek.gettime() + i * 7 * 24 * 60 * 60 * 1000);
//        const $carouselitem = $('<div class="carousel-item' + (i === 0 ? ' active' : '') + '"></div>');

//        for (let j = 0; j < 7; j++) {
//            const day = new date(weekstart.gettime() + j * 24 * 60 * 60 * 1000);
//            const $daymenu = $('<div class="day-menu"></div>');
//            $daymenu.append('<h3>' + day.tolocaledatestring() + '</h3>');
//            $daymenu.append('<p>a: soup 1</p>');
//            $daymenu.append('<p>b: soup 2</p>');
//            $daymenu.append('<p>c: main 1</p>');
//            $daymenu.append('<p>d: main 2</p>');
//            $daymenu.append('<p> f: dessert</p>');

//            $carouselitem.append($daymenu);
//        }

//        $('#weeklymenucarousel .carousel-inner').append($carouselitem);
//    }
//}
//$(document).ready(function () {
//    initializeweeklymenucarousel();
//});
async function fetchMenuItems() {
    try {
        const response = await fetch('/api/menuitems');
        const menuItems = await response.json();
        return menuItems;
    } catch (error) {
        console.error('Error fetching menu items:', error);
        return [];
    }
}

async function initializeWeeklyMenuCarousel() {
    console.log('Initializing weekly menu carousel');
    const weeksToShow = 3; // Number of weeks to display in the carousel
    const today = new Date();
    const startOfWeek = new Date(today.setDate(today.getDate() - today.getDay() + (today.getDay() === 0 ? -6 : 1)));
    const menuItems = await fetchMenuItems();

    for (let i = 0; i < weeksToShow; i++) {
        const weekStart = new Date(startOfWeek.getTime() + i * 7 * 24 * 60 * 60 * 1000);
        const $carouselItem = $('<div class="carousel-item' + (i === 0 ? ' active' : '') + '"></div>');

        for (let j = 0; j < 7; j++) {
            const day = new Date(weekStart.getTime() + j * 24 * 60 * 60 * 1000);
            const dateStr = day.toLocaleDateString();
            const dailyMenuItems = menuItems[dateStr] || {}; // Get menu items for the specific day or an empty object
            const $dayMenu = $('<div class="day-menu"></div>');
            $dayMenu.append('<h3>' + dateStr + '</h3>');

            if (dailyMenuItems.A) $dayMenu.append('<p>A: ' + dailyMenuItems.A + '</p>');
            if (dailyMenuItems.B) $dayMenu.append('<p>B: ' + dailyMenuItems.B + '</p>');
            if (dailyMenuItems.C) $dayMenu.append('<p>C: ' + dailyMenuItems.C + '</p>');
            if (dailyMenuItems.D) $dayMenu.append('<p>D: ' + dailyMenuItems.D + '</p>');
            if (dailyMenuItems.F) $dayMenu.append('<p>F: ' + dailyMenuItems.F + '</p>');

            $carouselItem.append($dayMenu);
        }

        $('#weeklyMenuCarousel .carousel-inner').append($carouselItem);
    }
}

$(document).ready(function () {
    initializeWeeklyMenuCarousel();
});
