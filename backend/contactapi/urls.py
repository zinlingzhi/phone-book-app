from django.urls import path
from . import views 

urlpatterns = [
    path('', views.home, name='home'),
    path('list-contact/', views.list_contact, name='list-contact'),
    path('search-contact/', views.search_contact, name='search-contact'),
    path('create-contact/', views.create_contact, name='create-contact'),
    path('delete-contact/<int:pk>/', views.delete_contact, name='delete-contact')
]