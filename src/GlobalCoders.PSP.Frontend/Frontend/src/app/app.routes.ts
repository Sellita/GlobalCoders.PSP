import { Routes } from '@angular/router';
import { TaxComponent } from '../components/tax/tax.component';
import { MerchantComponent } from '../components/merchant/merchant.component';
import { ProductComponent } from '../components/product/product.component';
import { ServiceComponent } from '../components/service/service.component';
import { UserComponent } from '../components/user/user.component';
import { OrderComponent } from '../components/order/order.component';
import { ReservationComponent } from '../components/reservation/reservation.component';
import { LoginComponent } from '../components/login/login.component';
import { DiscountComponent } from '../components/discount/discount.component';
import { authGuard } from '../services/auth.guard';

export const routes: Routes = [
    { path: '', component: OrderComponent, canActivate:[authGuard] }, // Ruta por defecto (Inicio)
    { path: 'tax', component: TaxComponent, canActivate:[authGuard]  }, // Ruta para "Impuestos"
    { path: 'merchant', component: MerchantComponent, canActivate:[authGuard]  }, // Ruta para "Comerciantes"
    { path: 'product', component: ProductComponent, canActivate:[authGuard]  }, // Ruta para "Productos"
    { path: 'service', component: ServiceComponent, canActivate:[authGuard]  }, // Ruta para "Servicios"
    { path: 'user', component: UserComponent, canActivate:[authGuard]  }, // Ruta para "Usuarios"
    { path: 'order', component: OrderComponent, canActivate:[authGuard] }, // Ruta para "Ordenes"
    { path: 'reservation', component: ReservationComponent, canActivate:[authGuard]  }, // Ruta para "Reservaciones"
    { path: 'login', component: LoginComponent }, // Ruta para "Login"
    { path: 'discount', component: DiscountComponent, canActivate:[authGuard]  } // Ruta para "Descuentos"

];
