import { useState, useEffect } from 'react';
import api from '../services/api';
import type { Competency } from '../types';

export default function Competencies() {
  const [competencies, setCompetencies] = useState<Competency[]>([]);
  const [expanded, setExpanded] = useState<Record<number, boolean>>({});

  useEffect(() => {
    const fetchCompetencies = async () => {
      try {
        const res = await api.get('/competencies');
        setCompetencies(res.data);
      } catch (error) {
        console.error('Ошибка загрузки компетенций', error);
      }
    };
    fetchCompetencies();
  }, []);

  const toggle = (id: number) => {
    setExpanded(prev => ({ ...prev, [id]: !prev[id] }));
  };

  return (
    <div className="section">
      <div className="section__header">
        <h2>Компетенции</h2>
        <p>Нажмите на заголовок, чтобы увидеть описание и скачать задание.</p>
      </div>
      <div className="accordionList">
        {competencies.map(comp => (
          <div key={comp.id} className="card accordionItem">
            <button className="accordionHeader" onClick={() => toggle(comp.id)}>
              <span>{comp.title}</span>
              <span>{expanded[comp.id] ? '−' : '+'}</span>
            </button>
            {expanded[comp.id] && (
              <div className="accordionBody">
                <p>{comp.description}</p>
                {comp.assignmentFileUrl && (
                  <a href={comp.assignmentFileUrl} target="_blank" rel="noopener noreferrer">
                    <button>Скачать задание</button>
                  </a>
                )}
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}