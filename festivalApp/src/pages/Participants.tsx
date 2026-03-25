import { useState, useEffect } from 'react';
import api from '../services/api';
import type { Participant, Region, Competency } from '../types';

export default function Participants() {
  const [participants, setParticipants] = useState<Participant[]>([]);
  const [regions, setRegions] = useState<Region[]>([]);
  const [competencies, setCompetencies] = useState<Competency[]>([]);
  const [filters, setFilters] = useState({
    name: '',
    competencyId: '',
    category: '',
    regionId: '',
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [regionsRes, compRes] = await Promise.all([
          api.get('/regions'),
          api.get('/competencies'),
        ]);
        setRegions(regionsRes.data);
        setCompetencies(compRes.data);
      } catch (error) {
        console.error('Ошибка загрузки фильтров', error);
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    const loadParticipants = async () => {
      try {
        const params = new URLSearchParams();
        if (filters.name) params.append('name', filters.name);
        if (filters.competencyId) params.append('competencyId', filters.competencyId);
        if (filters.category) params.append('category', filters.category);
        if (filters.regionId) params.append('regionId', filters.regionId);
        const res = await api.get(`/participants?${params.toString()}`);
        setParticipants(res.data);
      } catch (error) {
        console.error('Ошибка загрузки участников', error);
      }
    };
    loadParticipants();
  }, [filters]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFilters(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  return (
    <div className="section">
      <div className="section__header">
        <h2>Участники</h2>
        <p>Список участников с подтверждёнными заявками.</p>
      </div>
      <div className="card filters">
        <input
          type="text"
          name="name"
          placeholder="Поиск по имени"
          value={filters.name}
          onChange={handleChange}
        />
        <select name="competencyId" value={filters.competencyId} onChange={handleChange}>
          <option value="">Все компетенции</option>
          {competencies.map(c => (
            <option key={c.id} value={c.id}>{c.title}</option>
          ))}
        </select>
        <select name="category" value={filters.category} onChange={handleChange}>
          <option value="">Все категории</option>
          <option value="school">Школьник</option>
          <option value="student">Студент</option>
          <option value="specialist">Специалист</option>
        </select>
        <select name="regionId" value={filters.regionId} onChange={handleChange}>
          <option value="">Все регионы</option>
          {regions.map(r => (
            <option key={r.id} value={r.id}>{r.name}</option>
          ))}
        </select>
      </div>
      <div className="grid grid-3">
        {participants.length ? (
          participants.map(p => (
            <div className="card participant-card" key={p.fullName}>
              <img
                src={p.photoUrl || 'https://via.placeholder.com/300x200?text=Нет+фото'}
                alt={p.fullName}
              />
              <div className="participant-card__body">
                <h3>{p.fullName}</h3>
                <p><strong>Компетенция:</strong> {p.competency}</p>
                <p><strong>Регион:</strong> {p.region}</p>
                <p><strong>Категория:</strong> {p.category}</p>
              </div>
            </div>
          ))
        ) : (
          <div className="notice">Участники не найдены.</div>
        )}
      </div>
    </div>
  );
}