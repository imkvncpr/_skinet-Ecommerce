import { Component, inject, Input, input } from '@angular/core';
import { IProduct } from '../../../shared/models/IProduct';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [
      MatCard, MatCardContent,
      CurrencyPipe, MatCardActions, MatIcon,
      MatButton, RouterLink
    ],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
  @Input() product?: IProduct;
 cartService = inject(CartService);

}
