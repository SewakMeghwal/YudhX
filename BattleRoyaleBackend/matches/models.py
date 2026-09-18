from django.db import models
from django.contrib.auth.models import User

class MatchRecord(models.Model):
    match_id = models.CharField(max_length=64, unique=True)
    map_id = models.CharField(max_length=64, default='map_prototype_island')
    total_players = models.IntegerField(default=8)
    duration_seconds = models.FloatField(default=0.0)
    created_at = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return f"Match {self.match_id} ({self.total_players} players)"

class MatchParticipant(models.Model):
    match = models.ForeignKey(MatchRecord, on_delete=models.CASCADE, related_name='participants')
    user = models.ForeignKey(User, on_delete=models.CASCADE, related_name='match_history')
    placement_rank = models.IntegerField(default=1)
    kills = models.IntegerField(default=0)
    damage_dealt = models.FloatField(default=0.0)
    xp_earned = models.IntegerField(default=0)

    class Meta:
        ordering = ['placement_rank']

    def __str__(self):
        return f"{self.user.username} - Rank #{self.placement_rank} ({self.kills} kills)"
