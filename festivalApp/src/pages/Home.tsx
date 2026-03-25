import { useState, useEffect } from 'react';

const sliderImages = [
  {
    src: 'https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=1400&q=80',
    alt: 'Открытие фестиваля',
    title: 'Яркое открытие фестиваля',
    text: 'Мотивация, энергия, новые знакомства и профессиональный рост.',
  },
  {
    src: 'https://images.unsplash.com/photo-1522202176988-66273c2fd55f?auto=format&fit=crop&w=1400&q=80',
    alt: 'Командная работа участников',
    title: 'Командная работа и обмен опытом',
    text: 'Участники работают вместе, пробуют себя в новых ролях и направлениях.',
  },
  {
    src: 'https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=1400&q=80',
    alt: 'Современные технологии на фестивале',
    title: 'Технологии и возможности',
    text: 'Фестиваль объединяет цифровые компетенции, творчество и реальную практику.',
  },
];

export default function Home() {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [daysLeft, setDaysLeft] = useState(0);
  const festivalDate = new Date('2026-06-15T10:00:00');

  useEffect(() => {
    const timer = setInterval(() => {
      setCurrentSlide((prev) => (prev + 1) % sliderImages.length);
    }, 4000);
    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    const updateCounter = () => {
      const diff = festivalDate.getTime() - new Date().getTime();
      const days = Math.max(Math.ceil(diff / (1000 * 60 * 60 * 24)), 0);
      setDaysLeft(days);
    };
    updateCounter();
    const interval = setInterval(updateCounter, 60000);
    return () => clearInterval(interval);
  }, []);

  return (
    <>
      <section className="hero">
        <div className="hero__content card">
          <span className="badge">Фестиваль 2026</span>
          <h2>Открой способности. Найди свою компетенцию. Построй будущее.</h2>
          <p>
            Фестиваль возможностей объединяет школьников, студентов и специалистов,
            помогает развивать навыки, пробовать новое и находить своё направление.
          </p>
          <div className="hero__actions">
            <button onClick={() => window.location.href = '/register'}>Стать участником</button>
            <button className="secondary" onClick={() => window.location.href = '/competencies'}>
              Смотреть компетенции
            </button>
          </div>
          <div className="counterBox">
            <strong>{daysLeft}</strong>
            <span>дней до начала фестиваля</span>
          </div>
        </div>
        <div className="hero__slider card">
          <img src={sliderImages[currentSlide].src} alt={sliderImages[currentSlide].alt} />
          <div className="slideInfo">
            <h3>{sliderImages[currentSlide].title}</h3>
            <p>{sliderImages[currentSlide].text}</p>
          </div>
        </div>
      </section>

      <section className="section">
        <div className="section__header">
          <h2>Концепция и ценности</h2>
          <p>Мы создаём пространство, где знания, практика, технологии и творчество работают вместе.</p>
        </div>
        <div className="grid grid-4">
          <div className="card info-card">
            <h3>Развитие</h3>
            <p>Каждый участник получает шанс расти, пробовать и раскрывать талант.</p>
          </div>
          <div className="card info-card">
            <h3>Технологии</h3>
            <p>Актуальные направления, современные навыки и практические задачи.</p>
          </div>
          <div className="card info-card">
            <h3>Команда</h3>
            <p>Объединяем наставников, участников и организаторов в одно сообщество.</p>
          </div>
          <div className="card info-card">
            <h3>Возможности</h3>
            <p>Фестиваль помогает выбрать путь развития и сделать первый серьёзный шаг.</p>
          </div>
        </div>
      </section>

      <section className="section">
        <div className="section__header">
          <h2>Ключевые мероприятия</h2>
          <p>Основные дни и события фестиваля.</p>
        </div>
        <div className="grid grid-3">
          <div className="card event-card">
            <span className="event-date">15 июня 2026</span>
            <h3>Открытие</h3>
            <p>Знакомство с программой, старт фестиваля и приветствие участников.</p>
          </div>
          <div className="card event-card">
            <span className="event-date">16 июня 2026</span>
            <h3>Практика и мастер-классы</h3>
            <p>Воркшопы, задания, встречи с экспертами и работа по направлениям.</p>
          </div>
          <div className="card event-card">
            <span className="event-date">17 июня 2026</span>
            <h3>Финал и награждение</h3>
            <p>Подведение итогов, презентация результатов и награждение лучших.</p>
          </div>
        </div>
      </section>

      <section className="section">
        <div className="section__header">
          <h2>Организаторы и команда</h2>
          <p>Люди, которые делают фестиваль полезным и живым.</p>
        </div>
        <div className="grid grid-4">
          <div className="card info-card">
            <h3>Оргкомитет</h3>
            <p>Планирует программу и отвечает за общую организацию фестиваля.</p>
          </div>
          <div className="card info-card">
            <h3>Эксперты</h3>
            <p>Проводят занятия, оценивают работы и делятся опытом.</p>
          </div>
          <div className="card info-card">
            <h3>Наставники</h3>
            <p>Помогают участникам разбираться в заданиях и развиваться.</p>
          </div>
          <div className="card info-card">
            <h3>Волонтёры</h3>
            <p>Поддерживают гостей, сопровождают площадки и помогают в навигации.</p>
          </div>
        </div>
      </section>
    </>
  );
}