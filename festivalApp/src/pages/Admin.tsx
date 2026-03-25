import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import api from '../services/api';
import type { Competency } from '../types';

export default function Admin() {
  const { user } = useAuth();
  const [competencies, setCompetencies] = useState<Competency[]>([]);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [file, setFile] = useState<File | null>(null);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    fetchCompetencies();
  }, []);

  const fetchCompetencies = async () => {
    const res = await api.get('/competencies');
    setCompetencies(res.data);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!title || !description) {
      setError('Название и описание обязательны');
      return;
    }
    const formData = new FormData();
    formData.append('Title', title);
    formData.append('Description', description);
    if (file) formData.append('AssignmentFile', file);

    try {
      if (editingId) {
        await api.put(`/competencies/${editingId}`, formData, {
          headers: { 'Content-Type': 'multipart/form-data' },
        });
        setSuccess('Компетенция обновлена');
      } else {
        await api.post('/competencies', formData, {
          headers: { 'Content-Type': 'multipart/form-data' },
        });
        setSuccess('Компетенция добавлена');
      }
      setTitle('');
      setDescription('');
      setFile(null);
      setEditingId(null);
      fetchCompetencies();
      setTimeout(() => setSuccess(''), 3000);
    } catch (err: any) {
      setError(err.response?.data?.error || 'Ошибка сохранения');
    }
  };

  const startEdit = (comp: Competency) => {
    setEditingId(comp.id);
    setTitle(comp.title);
    setDescription(comp.description);
    setFile(null);
  };

  if (!user || user.role !== 'admin') {
    return <div className="notice">Доступ запрещён. Только для администраторов.</div>;
  }

  return (
    <div className="section narrow">
      <div className="section__header">
        <h2>Админ-панель</h2>
        <p>Управление компетенциями</p>
      </div>
      <form className="card form" onSubmit={handleSubmit}>
        <div className="field">
          <label>Название компетенции *</label>
          <input value={title} onChange={e => setTitle(e.target.value)} required />
        </div>
        <div className="field">
          <label>Описание *</label>
          <textarea value={description} onChange={e => setDescription(e.target.value)} rows={4} required />
        </div>
        <div className="field">
          <label>Файл задания</label>
          <input type="file" onChange={e => setFile(e.target.files?.[0] || null)} />
        </div>
        <button type="submit">
          {editingId ? 'Сохранить изменения' : 'Добавить компетенцию'}
        </button>
        {error && <p className="error">{error}</p>}
        {success && <p className="success">{success}</p>}
      </form>

      <div className="mt24">
        <h3 className="subTitle">Список компетенций</h3>
        <div className="accordionList">
          {competencies.map(comp => (
            <div className="card adminCompetency" key={comp.id}>
              <div>
                <h4>{comp.title}</h4>
                <p>{comp.description}</p>
                {comp.assignmentFileUrl && (
                  <a href={comp.assignmentFileUrl} target="_blank" rel="noopener noreferrer">
                    Скачать задание
                  </a>
                )}
              </div>
              <button onClick={() => startEdit(comp)}>Редактировать</button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}