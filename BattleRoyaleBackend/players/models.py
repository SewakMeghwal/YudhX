from django.db import models
from django.contrib.auth.models import User

class PlayerProfile(models.Model):
    user = models.OneToOneField(User, on_delete=models.CASCADE, related_name='profile')
    display_name = models.CharField(max_length=50, default='Survivor')
    avatar_url = models.CharField(max_length=255, blank=True, default='')
    level = models.IntegerField(default=1)
    total_xp = models.IntegerField(default=0)
    total_matches = models.IntegerField(default=0)
    total_wins = models.IntegerField(default=0)
    total_kills = models.IntegerField(default=0)
    total_deaths = models.IntegerField(default=0)
    total_damage = models.FloatField(default=0.0)
    updated_at = models.DateTimeField(auto_now=True)

    @property
    def kd_ratio(self):
        return round(self.total_kills / max(1, self.total_deaths), 2)

    @property
    def win_rate(self):
        return round((self.total_wins / max(1, self.total_matches)) * 100.0, 1)

    def __str__(self):
        return f"{self.display_name} (Lvl {self.level})"
