import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BaseModalComponent } from './base-modal.component';
import { By } from '@angular/platform-browser';

describe('BaseModalComponent', () => {
  let component: BaseModalComponent;
  let fixture: ComponentFixture<BaseModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaseModalComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(BaseModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display create title and icon when not editing', () => {
    component.isEditing = false;
    component.createTitle = 'Create Item';
    component.createIcon = 'bi-plus-circle';
    fixture.detectChanges();

    const titleElement = fixture.debugElement.query(By.css('.modal-title'));
    expect(titleElement.nativeElement.textContent.trim()).toBe('Create Item');
    expect(titleElement.query(By.css('i')).nativeElement.classList).toContain('bi-plus-circle');
  });

  it('should display edit title and icon when editing', () => {
    component.isEditing = true;
    component.editTitle = 'Edit Item';
    component.editIcon = 'bi-pencil';
    fixture.detectChanges();

    const titleElement = fixture.debugElement.query(By.css('.modal-title'));
    expect(titleElement.nativeElement.textContent.trim()).toBe('Edit Item');
    expect(titleElement.query(By.css('i')).nativeElement.classList).toContain('bi-pencil');
  });

  it('should emit cancel event when close button is clicked', () => {
    spyOn(component.cancel, 'emit');
    const closeButton = fixture.debugElement.query(By.css('.btn-close'));
    closeButton.triggerEventHandler('click', null);
    
    expect(component.cancel.emit).toHaveBeenCalled();
  });

  it('should have modal backdrop', () => {
    const backdrop = fixture.debugElement.query(By.css('.modal-backdrop'));
    expect(backdrop).toBeTruthy();
  });

  it('should have centered modal dialog', () => {
    const modalDialog = fixture.debugElement.query(By.css('.modal-dialog'));
    expect(modalDialog.nativeElement.classList).toContain('modal-dialog-centered');
  });

  it('should have primary background in header', () => {
    const modalHeader = fixture.debugElement.query(By.css('.modal-header'));
    expect(modalHeader.nativeElement.classList).toContain('bg-primary');
  });

  it('should have white text in header', () => {
    const modalHeader = fixture.debugElement.query(By.css('.modal-header'));
    expect(modalHeader.nativeElement.classList).toContain('text-white');
  });

  it('should have padding in modal body', () => {
    const modalBody = fixture.debugElement.query(By.css('.modal-body'));
    expect(modalBody.nativeElement.classList).toContain('p-4');
  });
}); 