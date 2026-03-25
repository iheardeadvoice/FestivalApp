import { useState } from 'react';
import api from '../services/api';

export default function Contacts() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [status, setStatus] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post('/contact', { name, email, message });
      setStatus('Сообщение отправлено');
      setName('');
      setEmail('');
      setMessage('');
      setTimeout(() => setStatus(''), 3000);
    } catch {
      setStatus('Ошибка отправки');
    }
  };

  return (
    <div className="section">
      <div className="section__header">
        <h2>Контакты</h2>
        <p>Свяжитесь с нами</p>
      </div>
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '2rem' }}>
        <div className="card info-card">
          <h3>Контактная информация</h3>
          <p>Email: info@festival.ru</p>
          <p>Телефон: +7 (495) 123-45-67</p>
          <p>Соцсети: Telegram, VK</p>
        </div>
        <form className="card form" onSubmit={handleSubmit}>
          <div className="field">
            <label>Ваше имя</label>
            <input value={name} onChange={e => setName(e.target.value)} required />
          </div>
          <div className="field">
            <label>Email</label>
            <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
          </div>
          <div className="field">
            <label>Сообщение</label>
            <textarea value={message} onChange={e => setMessage(e.target.value)} rows={4} required />
          </div>
          <button type="submit">Отправить</button>
          {status && <p className={status.includes('Ошибка') ? 'error' : 'success'}>{status}</p>}
        </form>
      </div>
    </div>
  );
}