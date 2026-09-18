from rest_framework import generics, status
from rest_framework.response import Response
from rest_framework.permissions import IsAuthenticated
from .models import MatchRecord, MatchParticipant
from .serializers import MatchRecordSerializer, MatchParticipantSerializer

class MatchListCreateView(generics.ListCreateAPIView):
    queryset = MatchRecord.objects.all().order_by('-created_at')
    serializer_class = MatchRecordSerializer
    permission_classes = (IsAuthenticated,)

class UserMatchHistoryView(generics.ListAPIView):
    serializer_class = MatchParticipantSerializer
    permission_classes = (IsAuthenticated,)

    def get_queryset(self):
        return MatchParticipant.objects.filter(user=self.request.user).order_by('-match__created_at')
