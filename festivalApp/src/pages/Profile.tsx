import { useState, useEffect, useMemo } from 'react';
import { useAuth } from '../contexts/AuthContext';
import api from '../services/api';
import type { Competency, Application } from '../types';

export default function Profile() {
  const { user } = useAuth();
  const [competencies, setCompetencies] = useState<Competency[]>([]);
  const [applications, setApplications] = useState<Application[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCompetencyId, setSelectedCompetencyId] = useState<number | null>(null);
  const [comment, setComment] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [compRes, appsRes] = await Promise.all([
          api.get('/competencies'),
          api.get('/applications/my'),
        ]);
        setCompetencies(compRes.data);
        setApplications(appsRes.data);
      } catch (err) {
        console.error('Ошибка загрузки данных', err);
      }
    };
    fetchData();
  }, []);

  const filteredCompetencies = useMemo(() => {
    if (!searchTerm) return competencies;
    return competencies.filter(c =>
      c.title.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [competencies, searchTerm]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedCompetencyId) {
      setError('Выберите компетенцию');
      return;
    }
    try {
      const res = await api.post('/applications', {
        competencyId: selectedCompetencyId,
        comment,
      });
      setApplications(prev => [res.data, ...prev]);
      setSelectedCompetencyId(null);
      setComment('');
      setSuccess('Заявка успешно подана');
      setTimeout(() => setSuccess(''), 3000);
    } catch (err: any) {
      setError(err.response?.data?.error || 'Ошибка при подаче заявки');
    }
  };

  if (!user) return <div className="notice">Необходимо авторизоваться</div>;

  return (
    <div className="section">
      <div className="section__header">
        <h2>Личный кабинет</h2>
        <p>Привет, {user.firstName} {user.lastName}!</p>
      </div>

      <div className="card searchBar">
        <input
          type="text"
          placeholder="Поиск компетенции по ключевым словам"
          value={searchTerm}
          onChange={e => setSearchTerm(e.target.value)}
        />
      </div>

      <div className="grid grid-3">
        {filteredCompetencies.map(c => (
          <div className="card info-card" key={c.id}>
            <h3>{c.title}</h3>
            <p>{c.description}</p>
            <button onClick={() => setSelectedCompetencyId(c.id)}>
              Выбрать
            </button>
          </div>
        ))}
      </div>

      <form className="card form mt24" onSubmit={handleSubmit}>
        <h3>Подача заявки</h3>
        <div className="field">
          <label>Выбранная компетенция</label>
          <input
            type="text"
            readOnly
            value={selectedCompetencyId
              ? competencies.find(c => c.id === selectedCompetencyId)?.title || ''
              : ''}
            placeholder="Не выбрано"
          />
        </div>
        <div className="field">
          <label>Комментарий</label>
          <textarea
            value={comment}
            onChange={e => setComment(e.target.value)}
            placeholder="Почему вы хотите участвовать?"
          />
        </div>
        <button type="submit">Отправить заявку</button>
        {error && <p className="error">{error}</p>}
        {success && <p className="success">{success}</p>}
      </form>

      <div className="mt24">
        <h3 className="subTitle">Мои заявки</h3>
        <div className="applicationsList">
          {applications.length ? (
            applications.map(app => (
              <div className="card application-card" key={app.id}>
                <h4>{app.competencyTitle}</h4>
                <p>{app.comment}</p>
                <span>Статус: {app.status === 'pending' ? 'На рассмотрении' :
                  app.status === 'approved' ? 'Одобрено' : 'Отклонено'}</span>
              </div>
            ))
          ) : (
            <div className="notice">У вас пока нет заявок.</div>
          )}
        </div>
      </div>
    </div>
  );
}