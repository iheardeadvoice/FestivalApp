import { Routes, Route, Link } from 'react-router-dom';
import { useAuth } from './contexts/AuthContext';
import { useState } from 'react';
import Home from './pages/Home';
import Competencies from './pages/Competencies';
import Participants from './pages/Participants';
import Contacts from './pages/Contacts';
import Register from './pages/Register';
import Login from './pages/Login';
import Profile from './pages/Profile';
import Admin from './pages/Admin';
import './App.css';

function App() {
  const { user, logout } = useAuth();
  const [darkTheme, setDarkTheme] = useState(false);
  const [bigText, setBigText] = useState(false);

  return (
    <div className={`app ${darkTheme ? 'dark' : ''} ${bigText ? 'big-text' : ''}`}>
      <div className="stars">
        {Array.from({ length: 18 }).map((_, i) => (
          <span key={i} className="star" />
        ))}
      </div>

      <header className="header">
        <div className="header__brand">
          <Link to="/">
            <div className="header__logo">ФВ</div>
            <div>
              <h1>Фестиваль возможностей</h1>
              <p>Современные компетенции, творчество и рост</p>
            </div>
          </Link>
        </div>

        <nav className="nav">
          <Link to="/"><button>Главная</button></Link>
          <Link to="/competencies"><button>Компетенции</button></Link>
          <Link to="/participants"><button>Участники</button></Link>
          <Link to="/contacts"><button>Контакты</button></Link>
          <Link to="/register"><button>Регистрация</button></Link>
          <Link to="/login"><button>Авторизация</button></Link>
          <Link to="/profile"><button>Личный кабинет</button></Link>
          {user?.role === 'admin' && <Link to="/admin"><button>Админ</button></Link>}
          <button onClick={() => setBigText(!bigText)}>Слабовидящим</button>
          <button onClick={() => setDarkTheme(!darkTheme)}>
            {darkTheme ? 'Светлая тема' : 'Тёмная тема'}
          </button>
          {user && <button onClick={logout}>Выйти</button>}
        </nav>
      </header>

      <main className="main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/competencies" element={<Competencies />} />
          <Route path="/participants" element={<Participants />} />
          <Route path="/contacts" element={<Contacts />} />
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/admin" element={<Admin />} />
        </Routes>
      </main>

      <footer className="footer">
        <div>
          <h3>Разделы</h3>
          <Link to="/">Главная</Link>
          <Link to="/competencies">Компетенции</Link>
          <Link to="/participants">Участники</Link>
          <Link to="/contacts">Контакты</Link>
          <Link to="/register">Регистрация</Link>
        </div>
        <div>
          <h3>Контакты</h3>
          <p>Телефон: +7 (495) 123-45-67</p>
          <p>Email: info@festival.ru</p>
          <p>VK • Telegram • YouTube</p>
        </div>
      </footer>
    </div>
  );
}

export default App;