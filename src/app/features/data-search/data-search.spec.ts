import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataSearch } from './data-search';

describe('DataSearch', () => {
  let component: DataSearch;
  let fixture: ComponentFixture<DataSearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataSearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DataSearch);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
