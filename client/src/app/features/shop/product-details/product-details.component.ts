import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from '../../../shared/models/IProduct';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CurrencyPipe, MatButton,
    MatIcon, MatFormField, 
    MatInput, MatLabel,
    MatDivider, FormsModule
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);
  product?: IProduct;
  quantityIncart = 0;
  quantity = 1;

ngOnInit() {
    this.loadProduct();
}

  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.shopService.getProduct(+id).subscribe({
        next: product => {
          this.product = product
          this.UpdateQuantity(1);
        },
        error: err => console.log(err)
      });
    }
  }

  updateCart() {
    if (!this.product) return;
    if(this.quantity > this.quantityIncart){
      const itemsToAdd = this.quantity - this.quantityIncart;
      this.quantity = this.quantityIncart + itemsToAdd;
      this.cartService.addItemToCart(this.product, itemsToAdd);
    } else {
      const itemsToRemove = this.quantityIncart - this.quantity;
      this.quantityIncart -= itemsToRemove;
      this.cartService.removeItemFromCart(this.product.id, itemsToRemove);
    }
      
  }

  UpdateQuantity(qty: number) {
    this.quantityIncart = this.cartService.cart()?.items.
    find(x => x.productId === this.product?.id)?.quantity || 0;
    this.quantity = this.quantityIncart || 1;
  }

  getButtonText(){
    return this.quantityIncart > 0 ? 'Update Cart' : 'Add to Cart'
  }
}
