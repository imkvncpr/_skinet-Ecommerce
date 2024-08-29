import { Component } from '@angular/core';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-redis-inspection',
  templateUrl: './redis-inspection.component.html',
  styleUrls: ['./redis-inspection.component.scss']
})
export class RedisInspectionComponent {
  constructor(private cartService: CartService) {}

  checkRedisHealth() {
    this.cartService.checkRedisHealth().subscribe({
      next: (result) => console.log('Redis health:', result),
      error: (error) => console.error('Error checking Redis health:', error)
    });
  }

  inspectCart(cartId: string) {
    this.cartService.inspectCart(cartId).subscribe({
      next: (result) => console.log('Cart contents:', result),
      error: (error) => console.error('Error inspecting cart:', error)
    });
  }
}