import bcrypt from 'bcryptjs';
import { useState } from "react";

export const API_BASE_URL = 'https://localhost:7281/api';
export let user = null;

export async function createUser(data) {
  const response = await fetch(`${API_BASE_URL}/Client/PostWithAddress`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la création de l\'utilisateur');
  }
  const user = await response.text();
  return user;
}

export async function getUser(email, password) {
  const encodedEmail = encodeURIComponent(email);
    const response = await fetch(`${API_BASE_URL}/Client/GetByEmail/${encodedEmail}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

  if (!response.ok) {
    throw console.log(response);

  }

  const userData = await response.json();
  console.log(userData);

  const connect = bcrypt.compare(password, userData.password).then((match) => {
    return !match;
  });

  return userData;
}
export async function useLogout() {
    localStorage.removeItem('account');
    window.location.reload();
}


