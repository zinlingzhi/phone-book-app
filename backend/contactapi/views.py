from django.shortcuts import render
from django.http import HttpResponse
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from contactapi.models import Contact
from contactapi.serializers import ContactSerializer
# Create your views here.


def home(request):
    return HttpResponse("My first api")

@api_view(['GET'])
def list_contact(request):
    contacts = Contact.objects.all()
    serializer = ContactSerializer(contacts, many=True)
    return Response(serializer.data, status=status.HTTP_200_OK)

@api_view(['POST'])
def search_contact(request):
    findText = request.data.get('findText')
    contacts = Contact.objects.all()
    querySets = contacts.filter(name__icontains=findText)
    serializer = ContactSerializer(querySets, many=True)
    return Response(serializer.data, status=status.HTTP_200_OK)

@api_view(['POST'])
def create_contact(request):
    serializer = ContactSerializer(data=request.data)

    if serializer.is_valid():
        serializer.save()
    return Response(serializer.data, status=status.HTTP_201_CREATED)

@api_view(['GET'])
def delete_contact(request, pk):
    contact = Contact.objects.get(id=pk)
    contact.delete()
    contacts = Contact.objects.all()
    serializer = ContactSerializer(contacts, many=True)
    return Response(serializer.data, status=status.HTTP_200_OK)

