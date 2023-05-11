// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    // Smooth scrolling for links with hashes
    $('a[href*="#"]:not([href="#"])').click(function () {
        if (
            location.pathname.replace(/^\//, '') ==
            this.pathname.replace(/^\//, '') &&
            location.hostname == this.hostname
        ) {
            var target = $(this.hash);
            target = target.length
                ? target
                : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate(
                    {
                        scrollTop: target.offset().top,
                    },
                    1000,
                    'easeInOutExpo'
                );
                return false;
            }
        }
    });

    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate(
            {
                scrollTop: 0,
            },
            1500,
            'easeInOutExpo'
        );
        return false;
    });

    
    // Header behavior
    const header = document.getElementById('header');
    const logo = document.getElementById('logo');
    const navigation = document.getElementById('navigation');
    const mobileMenuToggle = document.getElementById('mobile-menu-toggle');
    const navigationLinks = navigation.querySelectorAll('a');

    const handleScroll = () => {
        if (window.scrollY > 0) {
            header.classList.add('scrolled');
            navigation.classList.add('shrink');
            logo.style.height = '65px'; // Set the desired smaller height when scrolling
            header.style.height = '75px'; // Set the desired smaller height when scrolling
        } else {
            header.classList.remove('scrolled');
            navigation.classList.remove('shrink');
            logo.style.height = '90px'; // Set the original height when not scrolling
            header.style.height = '100px'; // Set the original height when not scrolling
        }
    };

    const toggleMobileMenu = () => {
        navigation.classList.toggle('show');
    };

    const closeMobileMenu = () => {
        if (window.innerWidth <= 767) {
            navigation.classList.remove('show');
        }
    };

    window.addEventListener('scroll', handleScroll);
    mobileMenuToggle.addEventListener('click', toggleMobileMenu);
    navigationLinks.forEach((link) => link.addEventListener('click', closeMobileMenu));
});

document.addEventListener('DOMContentLoaded', function () {
    var mobileMenuToggle = document.getElementById('mobile-menu-toggle');
    var navList = document.querySelector('.nav-list');
    var navLinks = document.querySelectorAll('.nav-list a');
    const menuItems = document.querySelectorAll(".navigation li a");

    mobileMenuToggle.addEventListener('click', function () {
        navList.classList.toggle('show');
    });

    navLinks.forEach(function (navLink) {
        navLink.addEventListener('click', function () {
            navList.classList.remove('show');
        });
    });

    menuItems.forEach(item => {
        item.addEventListener("click", function () {
            menuItems.forEach(i => i.classList.remove("active"));
            this.classList.add("active");
            window.location.hash = this.hash;
        });
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
/* 
    TYPER
*/
var Typer = function (element) {
    this.element = element;
    var delim = element.dataset.delim || ","; // default to comma
    var words = element.dataset.words || "override these,sample typing";
    this.words = words.split(delim).filter(function (v) { return v; }); // non empty words
    this.delay = element.dataset.delay || 200;
    this.loop = element.dataset.loop || "true";
    this.deleteDelay = element.dataset.deletedelay || element.dataset.deleteDelay || 800;

    this.progress = { word: 0, char: 0, building: true, atWordEnd: false, looped: 0 };
    this.typing = true;

    var colors = element.dataset.colors || "black";
    this.colors = colors.split(",");
    this.element.style.color = this.colors[0];
    this.colorIndex = 0;

    this.doTyping();
};

Typer.prototype.start = function () {
    if (!this.typing) {
        this.typing = true;
        this.doTyping();
    }
};
Typer.prototype.stop = function () {
    this.typing = false;
};
Typer.prototype.doTyping = function () {
    var e = this.element;
    var p = this.progress;
    var w = p.word;
    var c = p.char;
    var currentDisplay = [...this.words[w]].slice(0, c).join("");
    p.atWordEnd = false;
    if (this.cursor) {
        this.cursor.element.style.opacity = "1";
        this.cursor.on = true;
        clearInterval(this.cursor.interval);
        var itself = this.cursor;
        this.cursor.interval = setInterval(function () { itself.updateBlinkState(); }, 400);
    }

    e.innerHTML = currentDisplay;

    if (p.building) {
        if (p.char == [...this.words[w]].length) {
            p.building = false;
            p.atWordEnd = true;
        } else {
            p.char += 1;
        }
    } else {
        if (p.char == 0) {
            p.building = true;
            p.word = (p.word + 1) % this.words.length;
            this.colorIndex = (this.colorIndex + 1) % this.colors.length;
            this.element.style.color = this.colors[this.colorIndex];
        } else {
            p.char -= 1;
        }
    }

    if (p.atWordEnd) p.looped += 1;

    if (!p.building && (this.loop == "false" || this.loop <= p.looped)) {
        this.typing = false;
    }

    var myself = this;
    setTimeout(function () {
        if (myself.typing) { myself.doTyping(); };
    }, p.atWordEnd ? this.deleteDelay : this.delay);
};

var Cursor = function (element) {
    this.element = element;
    this.cursorDisplay = element.dataset.cursordisplay || "_";
    element.innerHTML = this.cursorDisplay;
    this.on = true;
    element.style.transition = "all 0.1s";
    var myself = this;
    this.interval = setInterval(function () {
        myself.updateBlinkState();
    }, 400);
}
Cursor.prototype.updateBlinkState = function () {
    if (this.on) {
        this.element.style.opacity = "0";
        this.on = false;
    } else {
        this.element.style.opacity = "1";
        this.on = true;
    }
}

function TyperSetup() {
    var typers = {};
    var elements = document.getElementsByClassName("typer");
    for (var i = 0, e; e = elements[i++];) {
        typers[e.id] = new Typer(e);
    }
    var elements = document.getElementsByClassName("typer-stop");
    for (var i = 0, e; e = elements[i++];) {
        let owner = typers[e.dataset.owner];
        e.onclick = function () { owner.stop(); };
    }
    var elements = document.getElementsByClassName("typer-start");
    for (var i = 0, e; e = elements[i++];) {
        let owner = typers[e.dataset.owner];
        e.onclick = function () { owner.start(); };
    }

    var elements2 = document.getElementsByClassName("cursor");
    for (var i = 0, e; e = elements2[i++];) {
        let t = new Cursor(e);
        t.owner = typers[e.dataset.owner];
        t.owner.cursor = t;
    }
}

TyperSetup();

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
//async function fetchMenuItems() {
//    try {
//        const response = await fetch('/api/menuitems');
//        const menuItems = await response.json();
//        return menuItems;
//    } catch (error) {
//        console.error('Error fetching menu items:', error);
//        return [];
//    }
//}

//async function initializeWeeklyMenuCarousel() {
//    console.log('Initializing weekly menu carousel');
//    const weeksToShow = 3; // Number of weeks to display in the carousel
//    const today = new Date();
//    const startOfWeek = new Date(today.setDate(today.getDate() - today.getDay() + (today.getDay() === 0 ? -6 : 1)));
//    const menuItems = await fetchMenuItems();

//    for (let i = 0; i < weeksToShow; i++) {
//        const weekStart = new Date(startOfWeek.getTime() + i * 7 * 24 * 60 * 60 * 1000);
//        const $carouselItem = $('<div class="carousel-item' + (i === 0 ? ' active' : '') + '"></div>');

//        for (let j = 0; j < 7; j++) {
//            const day = new Date(weekStart.getTime() + j * 24 * 60 * 60 * 1000);
//            const dateStr = day.toLocaleDateString();
//            const dailyMenuItems = menuItems[dateStr] || {}; // Get menu items for the specific day or an empty object
//            const $dayMenu = $('<div class="day-menu"></div>');
//            $dayMenu.append('<h3>' + dateStr + '</h3>');

//            if (dailyMenuItems.A) $dayMenu.append('<p>A: ' + dailyMenuItems.A + '</p>');
//            if (dailyMenuItems.B) $dayMenu.append('<p>B: ' + dailyMenuItems.B + '</p>');
//            if (dailyMenuItems.C) $dayMenu.append('<p>C: ' + dailyMenuItems.C + '</p>');
//            if (dailyMenuItems.D) $dayMenu.append('<p>D: ' + dailyMenuItems.D + '</p>');
//            if (dailyMenuItems.F) $dayMenu.append('<p>F: ' + dailyMenuItems.F + '</p>');

//            $carouselItem.append($dayMenu);
//        }

//        $('#weeklyMenuCarousel .carousel-inner').append($carouselItem);
//    }
//}

//$(document).ready(function () {
//    initializeWeeklyMenuCarousel();
//});
