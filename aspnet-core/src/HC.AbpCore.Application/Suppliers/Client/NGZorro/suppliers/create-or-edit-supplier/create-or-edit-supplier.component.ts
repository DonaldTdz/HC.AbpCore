
import { Component, OnInit, Injector, Input, ViewChild, AfterViewInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { CreateOrUpdateSupplierInput,SupplierEditDto, SupplierServiceProxy } from '@shared/service-proxies/service-proxies';
import { Validators, AbstractControl, FormControl } from '@angular/forms';

@Component({
  selector: 'create-or-edit-supplier',
  templateUrl: './create-or-edit-supplier.component.html',
  styleUrls:[
	'create-or-edit-supplier.component.less'
  ],
})

export class CreateOrEditSupplierComponent
  extends ModalComponentBase
    implements OnInit {
    /**
    * 编辑时DTO的id
    */
    id: any ;

	  entity: SupplierEditDto=new SupplierEditDto();

    /**
    * 初始化的构造函数
    */
    constructor(
		injector: Injector,
		private _supplierService: SupplierServiceProxy
	) {
		super(injector);
    }

    ngOnInit() :void{
		this.init();
    }


    /**
    * 初始化方法
    */
    init(): void {
		this._supplierService.getForEdit(this.id).subscribe(result => {
			this.entity = result.supplier;
		});
    }

    /**
    * 保存方法,提交form表单
    */
    submitForm(): void {
		const input = new CreateOrUpdateSupplierInput();
		input.supplier = this.entity;

		this.saving = true;

		this._supplierService.createOrUpdate(input)
		.finally(() => (this.saving = false))
		.subscribe(() => {
			this.notify.success(this.l('SavedSuccessfully'));
			this.success(true);
		});
    }
}
