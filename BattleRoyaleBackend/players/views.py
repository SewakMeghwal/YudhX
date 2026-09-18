from rest_framework import generics
from rest_framework.permissions import IsAuthenticated
from .models import PlayerProfile
from .serializers import PlayerProfileSerializer

class PlayerProfileDetailView(generics.RetrieveUpdateAPIView):
    serializer_class = PlayerProfileSerializer
    permission_classes = (IsAuthenticated,)

    def get_object(self):
        profile, _ = PlayerProfile.objects.get_or_create(user=self.request.user)
        return profile
