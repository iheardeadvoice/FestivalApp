import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import type { Region } from '../types';

const schema = z.object({
  lastName: z.string().min(1, 'Фамилия обязательна'),
  firstName: z.string().min(1, 'Имя обязательно'),
  patronymic: z.string().optional(),
  email: z.string().email('Некорректный email'),
  password: z.string().min(8, 'Пароль не менее 8 символов'),
  confirmPassword: z.string(),
  education: z.string().min(1, 'Образование обязательно'),
  institution: z.string().min(1, 'Учебное заведение обязательно'),
  regionId: z.string().min(1, 'Выберите регион'),
  category: z.enum(['school', 'student', 'specialist']),
  photo: z.any().optional(),
}).refine(data => data.password === data.confirmPassword, {
  message: 'Пароли не совпадают',
  path: ['confirmPassword'],
});

export default function Register() {
  const { register: registerUser } = useAuth();
  const navigate = useNavigate();
  const [regions, setRegions] = useState<Region[]>([]);
  const [error, setError] = useState('');
  const { register, handleSubmit, formState: { errors }, setValue } = useForm({
    resolver: zodResolver(schema),
  });

  useEffect(() => {
    const fetchRegions = async () => {
      try {
        const res = await api.get('/regions');
        setRegions(res.data);
      } catch {
        setRegions([
          { id: 1, name: 'Москва' },
          { id: 2, name: 'Санкт-Петербург' },
          { id: 3, name: 'Новосибирск' },
          { id: 4, name: 'Екатеринбург' },
          { id: 5, name: 'Казань' },
        ]);
      }
    };
    fetchRegions();
  }, []);

  const onSubmit = async (data: any) => {
    const formData = new FormData();
    formData.append('LastName', data.lastName);
    formData.append('FirstName', data.firstName);
    if (data.patronymic) formData.append('Patronymic', data.patronymic);
    formData.append('Email', data.email);
    formData.append('Password', data.password);
    formData.append('Education', data.education);
    formData.append('Institution', data.institution);
    formData.append('RegionId', data.regionId);
    formData.append('Category', data.category);
    if (data.photo && data.photo[0]) {
      formData.append('Photo', data.photo[0]);
    }

    const result = await registerUser(formData);
    if (result.success) {
      navigate('/profile');
    } else {
      setError(result.error || 'Ошибка регистрации');
    }
  };

  return (
    <div className="section narrow">
      <div className="section__header">
        <h2>Регистрация</h2>
        <p>Заполните форму для создания аккаунта.</p>
      </div>
      <form className="card form" onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
        <div className="formGrid">
          <div className="field">
            <label>Фамилия *</label>
            <input {...register('lastName')} />
            {errors.lastName && <span className="error">{String(errors.lastName.message)}</span>}
          </div>
          <div className="field">
            <label>Имя *</label>
            <input {...register('firstName')} />
            {errors.firstName && <span className="error">{String(errors.firstName.message)}</span>}
          </div>
          <div className="field">
            <label>Отчество</label>
            <input {...register('patronymic')} />
          </div>
          <div className="field">
            <label>Email *</label>
            <input type="email" {...register('email')} />
            {errors.email && <span className="error">{String(errors.email.message)}</span>}
          </div>
          <div className="field">
            <label>Пароль *</label>
            <input type="password" {...register('password')} />
            {errors.password && <span className="error">{String(errors.password.message)}</span>}
          </div>
          <div className="field">
            <label>Подтверждение пароля *</label>
            <input type="password" {...register('confirmPassword')} />
            {errors.confirmPassword && <span className="error">{String(errors.confirmPassword.message)}</span>}
          </div>
          <div className="field">
            <label>Образование *</label>
            <select {...register('education')}>
              <option value="">Выберите</option>
              <option value="школьник">Школьник</option>
              <option value="студент">Студент</option>
              <option value="специалист">Специалист</option>
            </select>
            {errors.education && <span className="error">{String(errors.education.message)}</span>}
          </div>
          <div className="field">
            <label>Учебное заведение *</label>
            <input {...register('institution')} />
            {errors.institution && <span className="error">{String(errors.institution.message)}</span>}
          </div>
          <div className="field">
            <label>Регион *</label>
            <select {...register('regionId')}>
              <option value="">Выберите регион</option>
              {regions.map(r => (
                <option key={r.id} value={r.id}>{r.name}</option>
              ))}
            </select>
            {errors.regionId && <span className="error">{String(errors.regionId.message)}</span>}
          </div>
          <div className="field">
            <label>Категория *</label>
            <select {...register('category')}>
              <option value="">Выберите категорию</option>
              <option value="school">Школьник</option>
              <option value="student">Студент</option>
              <option value="specialist">Специалист</option>
            </select>
            {errors.category && <span className="error">{String(errors.category.message)}</span>}
          </div>
          <div className="field">
            <label>Фото *</label>
            <input type="file" accept="image/*" onChange={(e) => setValue('photo', e.target.files)} />
            {errors.photo && <span className="error">{String(errors.photo.message)}</span>}
          </div>
        </div>
        <button type="submit">Зарегистрироваться</button>
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  );
}