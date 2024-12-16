import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Obtener el token de autenticación de localStorage (o de donde lo tengas)
  const authService = inject(AuthService);
  const token = localStorage.getItem('accessToken');
  
  // Si hay un token, agregarlo al encabezado Authorization
  if (token) {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
    
    // Continuar con la solicitud HTTP utilizando el token
    return next(clonedRequest);
  }
  
  // Si no hay token, continuar con la solicitud sin cambios
  return next(req);
};
