from rest_framework import generics
from rest_framework.permissions import AllowAny
from players.models import PlayerProfile
from players.serializers import PlayerProfileSerializer

class TopWinsLeaderboardView(generics.ListAPIView):
    serializer_class = PlayerProfileSerializer
    permission_classes = (AllowAny,)

    def get_queryset(self):
        return PlayerProfile.objects.all().order_by('-total_wins', '-total_kills')[:50]

class TopKillsLeaderboardView(generics.ListAPIView):
    serializer_class = PlayerProfileSerializer
    permission_classes = (AllowAny,)

    def get_queryset(self):
        return PlayerProfile.objects.all().order_by('-total_kills', '-total_wins')[:50]
