import bcrypt from "bcryptjs";

export const API_BASE_URL = 'https://localhost:7281/api';
export let user = null;

export async function Logout() {
  localStorage.removeItem('connected');
  window.location.reload();
}

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

    return await response.text();
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
    throw new Error('Erreur lors de la récupération de l\'utilisateur');
  }

  const userData = await response.json();
  console.log(userData);

  const connect = bcrypt.compare(password, userData.password).then((match) => {
    return !match;
  });

  return userData;
}

export async function getAddresses(id) {
  const response = await fetch(`${API_BASE_URL}/Address/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des adresses');
  }
  return await response.json();
}

export async function getOrders(id) {
  const response = await fetch(`${API_BASE_URL}/ClientOrder/GetOrdersByClient/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des commandes');
  }
  return await response.json();
}

