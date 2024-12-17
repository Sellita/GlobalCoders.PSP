import { Routes } from '@angular/router';
import { DashboardComponent } from '../components/dashboard/dashboard.component';
import { OrdersComponent } from '../components/orders/orders.component';
import { ReservationsComponent } from '../components/reservations/reservations.component';
import { PaymentsComponent } from '../components/payments/payments.component';
import { DiscountsComponent } from '../components/discounts/discounts.component';
import { TaxesComponent } from '../components/taxes/taxes.component';
import { ProductsComponent } from '../components/products/products.component';
import { MerchantsComponent } from '../components/merchants/merchants.component';
import { UsersComponent } from '../components/users/users.component';
import { ServicesComponent } from '../components/services/services.component';


export const routes: Routes = [
    { path: '', component: DashboardComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'orders', component: OrdersComponent },
    { path: 'reservations', component: ReservationsComponent },
    { path: 'payments', component: PaymentsComponent },
    { path: 'discounts', component: DiscountsComponent },
    { path: 'taxes', component: TaxesComponent },
    { path: 'products', component: ProductsComponent },
    { path: 'merchants', component: MerchantsComponent },
    { path: 'users', component: UsersComponent },
    { path: 'services', component: ServicesComponent }
];
