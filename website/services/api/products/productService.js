const API_BASE_URL = 'https://localhost:7281/api';

export async function getProducts() {
  const response = await fetch(`${API_BASE_URL}/Product`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des produits');
  }

  return await response.json();
}

export async function getFamily() {
  const response = await fetch(`${API_BASE_URL}/Family`, {

    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des familles');
  }

  return await response.json();
}
  
  export async function getProductByID(id) {
    const response = await fetch(`${API_BASE_URL}/Product/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    if (!response.ok) {
      throw new Error('Erreur lors de la récupération des produits');
    }

    return await response.json();
  }
    
export async function getFamilyById(id) {
  const response = await fetch(`${API_BASE_URL}/Product/GetProductByFamily/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération de la famille');
  }

  return await response.json();
}