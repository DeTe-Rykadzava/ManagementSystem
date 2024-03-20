// Получаем элементы слайдера
let slider = document.querySelector('.slider');
let prevButton = document.querySelector('.prev-button');
let nextButton = document.querySelector('.next-button');
let slides = Array.from(slider.querySelectorAll('img'));
let slideCount = slides.length;
let slideIndex = 0;

function initSlider()
{
    slider = document.querySelector('.slider');
    prevButton = document.querySelector('.prev-button');
    nextButton = document.querySelector('.next-button');
    slides = Array.from(slider.querySelectorAll('img'));
    slideCount = slides.length;
    slideIndex = 0;
}

// Устанавливаем обработчики событий для кнопок
prevButton.addEventListener('click', showPreviousSlide);
nextButton.addEventListener('click', showNextSlide);

// Функция для показа предыдущего слайда
function showPreviousSlide() {
    console.log('preview');
    slideIndex = slideIndex - 1;
    if (slideIndex < 0)
        slideIndex = slideCount - 1;
    updateSlider();
}

// Функция для показа следующего слайда
function showNextSlide() {
    console.log('next');
    slideIndex = slideIndex + 1;
    if (slideIndex > slideCount - 1)
        slideIndex = 0;
    updateSlider();
}

// Функция для обновления отображения слайдера
function updateSlider() {
    console.log('update slider');
    slides.forEach((slide, index) => {
        if (index === slideIndex) {
            slide.style.display = 'block';
        } else {
            slide.style.display = 'none';
        }
    });
}

// Инициализация слайдера
updateSlider();