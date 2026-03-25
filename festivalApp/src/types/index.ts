export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  patronymic?: string;
  education: string;
  institution: string;
  regionId: number;
  regionName?: string;
  photoUrl?: string;
  category: string;
  role: string;
}

export interface Region {
  id: number;
  name: string;
}

export interface Competency {
  id: number;
  title: string;
  description: string;
  assignmentFileUrl?: string;
}

export interface Application {
  id: number;
  competencyId: number;
  competencyTitle: string;
  status: 'pending' | 'approved' | 'rejected';
  comment?: string;
  createdAt: string;
}

export interface Participant {
  fullName: string;
  competency: string;
  region: string;
  category: string;
  photoUrl?: string;
}